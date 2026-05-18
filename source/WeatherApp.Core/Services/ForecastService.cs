using Microsoft.ML;
using Microsoft.ML.TimeSeries;
using Microsoft.ML.Transforms.TimeSeries;
using System.Text.Json;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services;

public sealed class ForecastService
{
    private readonly ModelProfileService _profileService;
    private readonly MLContext _mlContext = new(seed: 1);

    public ForecastService(ModelProfileService profileService)
    {
        _profileService = profileService;
    }

    public IReadOnlyList<ForecastPoint> Forecast(ModelProfile profile, int startOffsetHours, int horizon)
    {
        if (profile.LastTrainEndUtc is null)
        {
            throw new InvalidOperationException("Профиль не имеет истории обучения.");
        }

        if (profile.ModelType == ModelType.Boosting)
        {
            return ForecastBoosting(profile, startOffsetHours, horizon);
        }

        if (startOffsetHours < 0)
        {
            startOffsetHours = 0;
        }

        var effectiveHorizon = Math.Min(horizon, profile.Horizon - startOffsetHours);
        if (effectiveHorizon <= 0)
        {
            return Array.Empty<ForecastPoint>();
        }

        var startTime = profile.LastTrainEndUtc.Value.AddHours(1 + startOffsetHours);
        var points = new List<ForecastPoint>(effectiveHorizon);
        for (var i = 0; i < effectiveHorizon; i++)
        {
            points.Add(new ForecastPoint
            {
                Time = startTime.AddHours(i)
            });
        }

        ApplyMetricForecast(points, profile, startOffsetHours, MetricType.TemperatureC, (p, v) => p.TemperatureC = v);
        ApplyMetricForecast(points, profile, startOffsetHours, MetricType.Humidity, (p, v) => p.Humidity = v);
        ApplyMetricForecast(points, profile, startOffsetHours, MetricType.Pressure, (p, v) => p.Pressure = v);
        ApplyMetricForecast(points, profile, startOffsetHours, MetricType.WindSpeed, (p, v) => p.WindSpeed = v);

        return points;
    }

    private void ApplyMetricForecast(
        List<ForecastPoint> points,
        ModelProfile profile,
        int startOffsetHours,
        MetricType metric,
        Action<ForecastPoint, float> applyValue)
    {
        var modelPath = _profileService.GetModelPath(profile.Id, metric);
        if (!File.Exists(modelPath))
        {
            return;
        }

        var model = _mlContext.Model.Load(modelPath, out _);
        var engine = model.CreateTimeSeriesEngine<TimeSeriesData, ForecastOutput>(_mlContext);
        var forecast = engine.Predict();

        if (startOffsetHours >= forecast.Forecasted.Length)
        {
            return;
        }

        var length = Math.Min(points.Count, forecast.Forecasted.Length - startOffsetHours);
        for (var i = 0; i < length; i++)
        {
            applyValue(points[i], forecast.Forecasted[i + startOffsetHours]);
        }
    }

    private IReadOnlyList<ForecastPoint> ForecastBoosting(ModelProfile profile, int startOffsetHours, int horizon)
    {
        if (startOffsetHours < 0)
        {
            startOffsetHours = 0;
        }

        if (horizon <= 0)
        {
            return Array.Empty<ForecastPoint>();
        }

        var startTime = profile.LastTrainEndUtc!.Value.AddHours(1 + startOffsetHours);
        var points = new List<ForecastPoint>(horizon);
        for (var i = 0; i < horizon; i++)
        {
            points.Add(new ForecastPoint
            {
                Time = startTime.AddHours(i)
            });
        }

        ApplyBoostingMetric(points, profile, startOffsetHours, MetricType.TemperatureC, (p, v) => p.TemperatureC = v);
        ApplyBoostingMetric(points, profile, startOffsetHours, MetricType.Humidity, (p, v) => p.Humidity = v);
        ApplyBoostingMetric(points, profile, startOffsetHours, MetricType.Pressure, (p, v) => p.Pressure = v);
        ApplyBoostingMetric(points, profile, startOffsetHours, MetricType.WindSpeed, (p, v) => p.WindSpeed = v);

        return points;
    }

    private void ApplyBoostingMetric(
        List<ForecastPoint> points,
        ModelProfile profile,
        int startOffsetHours,
        MetricType metric,
        Action<ForecastPoint, float> applyValue)
    {
        var modelPath = _profileService.GetModelPath(profile.Id, metric);
        var historyPath = _profileService.GetBoostingHistoryPath(profile.Id, metric);
        if (!File.Exists(modelPath) || !File.Exists(historyPath))
        {
            return;
        }

        var historyJson = File.ReadAllText(historyPath);
        var history = JsonSerializer.Deserialize<BoostingHistory>(historyJson);
        if (history is null || history.LastValues.Length < BoostingConstants.LagCount)
        {
            return;
        }

        var model = _mlContext.Model.Load(modelPath, out _);
        var engine = _mlContext.Model.CreatePredictionEngine<BoostingInput, BoostingPrediction>(model);

        var lagQueue = new Queue<float>(history.LastValues);
        var totalSteps = startOffsetHours + points.Count;
        var generated = new List<float>(totalSteps);
        var currentTime = history.LastTimeUtc;

        for (var step = 0; step < totalSteps; step++)
        {
            currentTime = currentTime.AddHours(1);
            var features = BuildBoostingFeatures(lagQueue, currentTime);
            var prediction = engine.Predict(new BoostingInput { Features = features });
            var value = ClampBoostingValue(metric, prediction.Score, history);
            generated.Add(value);
            lagQueue.Dequeue();
            lagQueue.Enqueue(value);
        }

        generated = SmoothSeries(generated, 3);

        var startIndex = startOffsetHours;
        var length = Math.Min(points.Count, generated.Count - startIndex);
        for (var i = 0; i < length; i++)
        {
            applyValue(points[i], generated[startIndex + i]);
        }
    }

    private float[] BuildBoostingFeatures(Queue<float> lags, DateTime time)
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

        foreach (var lag in lags)
        {
            features[pos++] = lag;
        }

        return features;
    }

    private float ClampBoostingValue(MetricType metric, float value, BoostingHistory history)
    {
        var range = history.MaxValue - history.MinValue;
        var margin = Math.Max(2f, range * 0.1f);
        var min = history.MinValue - margin;
        var max = history.MaxValue + margin;

        switch (metric)
        {
            case MetricType.Humidity:
                min = Math.Max(min, 0f);
                max = Math.Min(max, 100f);
                break;
            case MetricType.WindSpeed:
                min = Math.Max(min, 0f);
                break;
        }

        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    private List<float> SmoothSeries(List<float> values, int window)
    {
        if (window <= 1 || values.Count <= 2)
        {
            return values;
        }

        var result = new List<float>(values.Count);
        var queue = new Queue<float>();
        float sum = 0f;

        for (var i = 0; i < values.Count; i++)
        {
            queue.Enqueue(values[i]);
            sum += values[i];
            if (queue.Count > window)
            {
                sum -= queue.Dequeue();
            }

            result.Add(sum / queue.Count);
        }

        return result;
    }
}
