using Microsoft.ML;
using Microsoft.ML.TimeSeries;
using System.Text.Json;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services;

public sealed class TrainingService
{
    private readonly WeatherApiService _apiService;
    private readonly DatasetBuilder _datasetBuilder;
    private readonly ModelProfileService _profileService;
    private readonly MLContext _mlContext = new(seed: 1);

    public TrainingService(
        WeatherApiService apiService,
        DatasetBuilder datasetBuilder,
        ModelProfileService profileService)
    {
        _apiService = apiService;
        _datasetBuilder = datasetBuilder;
        _profileService = profileService;
    }

    public async Task<TrainingResult> TrainAsync(
        ModelProfile profile,
        string city,
        DateTime startUtc,
        DateTime endUtc,
        bool fineTune,
        CancellationToken ct = default)
    {
        if (endUtc <= startUtc)
        {
            return new TrainingResult { Success = false, Message = "Дата окончания должна быть позже даты начала." };
        }

        var location = await ResolveLocationAsync(profile, city, ct).ConfigureAwait(false);
        if (location is null)
        {
            return new TrainingResult { Success = false, Message = "Город не найден в Open-Meteo." };
        }

        if (fineTune && profile.LastTrainStartUtc.HasValue && profile.LastTrainEndUtc.HasValue)
        {
            startUtc = startUtc < profile.LastTrainStartUtc.Value ? startUtc : profile.LastTrainStartUtc.Value;
            endUtc = endUtc > profile.LastTrainEndUtc.Value ? endUtc : profile.LastTrainEndUtc.Value;
        }

        var data = await _apiService.GetHistoricalHourlyAsync(
            location.Latitude,
            location.Longitude,
            startUtc,
            endUtc,
            ct).ConfigureAwait(false);

        return await TrainWithDataAsync(profile, location, startUtc, endUtc, data).ConfigureAwait(false);
    }

    public Task<TrainingResult> TrainWithDataAsync(
        ModelProfile profile,
        WeatherApiService.GeoLocation location,
        DateTime startUtc,
        DateTime endUtc,
        IReadOnlyList<WeatherDataPoint> data)
    {
        if (profile.ModelType == ModelType.Ssa && data.Count < profile.WindowSize + profile.Horizon)
        {
            return Task.FromResult(new TrainingResult
            {
                Success = false,
                Message = "Недостаточно данных для выбранного окна и горизонта."
            });
        }

        var orderedData = data.OrderBy(p => p.Time).ToList();
        if (profile.ModelType == ModelType.Boosting && orderedData.Count <= BoostingConstants.LagCount)
        {
            return Task.FromResult(new TrainingResult
            {
                Success = false,
                Message = $"Недостаточно данных: требуется минимум {BoostingConstants.LagCount + 1} часов."
            });
        }

        var series = _datasetBuilder.BuildSeries(orderedData);
        foreach (var metric in series.Keys)
        {
            var metricSeries = series[metric];
            var modelPath = _profileService.GetModelPath(profile.Id, metric);
            if (profile.ModelType == ModelType.Boosting)
            {
                TrainBoostingMetric(metricSeries, orderedData, modelPath);
                SaveBoostingHistory(profile.Id, metric, orderedData, metricSeries);
            }
            else
            {
                TrainMetric(metricSeries, profile, modelPath);
            }
        }

        profile.LocationName = location.Name;
        profile.Latitude = location.Latitude;
        profile.Longitude = location.Longitude;
        profile.LastTrainStartUtc = startUtc;
        profile.LastTrainEndUtc = endUtc;
        _profileService.UpdateProfile(profile);

        return Task.FromResult(new TrainingResult { Success = true, Message = "Обучение завершено." });
    }

    private void TrainMetric(List<float> series, ModelProfile profile, string modelPath)
    {
        var trainSize = profile.TrainSize > 0 && profile.TrainSize <= series.Count
            ? profile.TrainSize
            : series.Count;

        var dataView = _mlContext.Data.LoadFromEnumerable(series.Select(value => new TimeSeriesData
        {
            Value = value
        }));

        var pipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: nameof(ForecastOutput.Forecasted),
            inputColumnName: nameof(TimeSeriesData.Value),
            windowSize: profile.WindowSize,
            seriesLength: profile.SeriesLength,
            trainSize: trainSize,
            horizon: profile.Horizon,
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: nameof(ForecastOutput.LowerBound),
            confidenceUpperBoundColumn: nameof(ForecastOutput.UpperBound));

        var model = pipeline.Fit(dataView);
        _mlContext.Model.Save(model, dataView.Schema, modelPath);
    }

    private void TrainBoostingMetric(
        List<float> series,
        IReadOnlyList<WeatherDataPoint> orderedData,
        string modelPath)
    {
        var rows = new List<BoostingInput>();
        for (var i = BoostingConstants.LagCount; i < series.Count; i++)
        {
            var time = orderedData[i].Time;
            var features = BuildBoostingFeatures(series, i, time);
            rows.Add(new BoostingInput
            {
                Features = features,
                Label = series[i]
            });
        }

        var dataView = _mlContext.Data.LoadFromEnumerable(rows);
        var pipeline = _mlContext.Regression.Trainers.FastTree(new Microsoft.ML.Trainers.FastTree.FastTreeRegressionTrainer.Options
        {
            LabelColumnName = nameof(BoostingInput.Label),
            FeatureColumnName = nameof(BoostingInput.Features),
            NumberOfTrees = 200,
            NumberOfLeaves = 64,
            MinimumExampleCountPerLeaf = 20,
            LearningRate = 0.2
        });

        var model = pipeline.Fit(dataView);
        _mlContext.Model.Save(model, dataView.Schema, modelPath);
    }

    private float[] BuildBoostingFeatures(List<float> series, int index, DateTime time)
    {
        var features = new float[BoostingConstants.FeatureCount];
        var pos = 0;

        var hourAngle = 2f * (float)Math.PI * time.Hour / 24f;
        var dayAngle = 2f * (float)Math.PI * time.DayOfYear / 365f;

        features[pos++] = MathF.Sin(hourAngle);
        features[pos++] = MathF.Cos(hourAngle);
        features[pos++] = MathF.Sin(dayAngle);
        features[pos++] = MathF.Cos(dayAngle);
        features[pos++] = (int)time.DayOfWeek;
        features[pos++] = time.Month;

        for (var lag = 1; lag <= BoostingConstants.LagCount; lag++)
        {
            features[pos++] = series[index - lag];
        }

        return features;
    }

    private void SaveBoostingHistory(
        string profileId,
        MetricType metric,
        IReadOnlyList<WeatherDataPoint> orderedData,
        List<float> series)
    {
        var minValue = series.Min();
        var maxValue = series.Max();
        var history = new BoostingHistory
        {
            LastTimeUtc = orderedData.Last().Time,
            LastValues = series.TakeLast(BoostingConstants.LagCount).ToArray(),
            MinValue = minValue,
            MaxValue = maxValue
        };

        var path = _profileService.GetBoostingHistoryPath(profileId, metric);
        var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private async Task<WeatherApiService.GeoLocation?> ResolveLocationAsync(
        ModelProfile profile,
        string city,
        CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(city))
        {
            return await _apiService.GetLocationAsync(city, ct).ConfigureAwait(false);
        }

        if (profile.Latitude.HasValue && profile.Longitude.HasValue)
        {
            return new WeatherApiService.GeoLocation
            {
                Name = profile.LocationName ?? "Сохраненное место",
                Latitude = profile.Latitude.Value,
                Longitude = profile.Longitude.Value
            };
        }

        return null;
    }
}
