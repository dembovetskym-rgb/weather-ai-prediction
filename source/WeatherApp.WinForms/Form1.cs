using WeatherApp.Core.Models;
using WeatherApp.Core.Services;

namespace WeatherApp.WinForms;

public partial class Form1 : Form
{
    private readonly WeatherApiService _apiService;
    private readonly ModelProfileService _profileService;
    private readonly TrainingService _trainingService;
    private readonly ForecastService _forecastService;
    private readonly DatasetBuilder _datasetBuilder;

    public Form1()
    {
        InitializeComponent();

        var rootPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WeatherAppML");

        _apiService = new WeatherApiService(new HttpClient());
        _profileService = new ModelProfileService(rootPath);
        _datasetBuilder = new DatasetBuilder();
        _trainingService = new TrainingService(_apiService, _datasetBuilder, _profileService);
        _forecastService = new ForecastService(_profileService);

        cmbModelType.DataSource = new[]
        {
            new { Value = ModelType.Ssa, Text = "SSA (короткий прогноз)" },
            new { Value = ModelType.Boosting, Text = "Бустинг (дальние даты)" }
        };
        cmbModelType.DisplayMember = "Text";
        cmbModelType.ValueMember = "Value";

        dtTrainStart.Value = DateTime.Today.AddDays(-30);
        dtTrainEnd.Value = DateTime.Today;
        dtForecastDate.Value = DateTime.Today.AddDays(1);

        LoadProfiles();
    }

    private void LoadProfiles()
    {
        var profiles = _profileService.GetProfiles().OrderBy(p => p.Name).ToList();
        lstProfiles.DataSource = null;
        lstProfiles.DisplayMember = nameof(ModelProfile.Name);
        lstProfiles.ValueMember = nameof(ModelProfile.Id);
        lstProfiles.DataSource = profiles;

        var active = _profileService.GetActiveProfile();
        lblActiveProfile.Text = active is null ? "Активный: нет" : $"Активный: {active.Name}";
        if (active is not null)
        {
            var index = profiles.FindIndex(p => p.Id == active.Id);
            if (index >= 0)
            {
                lstProfiles.SelectedIndex = index;
            }
        }
    }

    private ModelProfile? GetSelectedProfile()
    {
        return lstProfiles.SelectedItem as ModelProfile;
    }

    private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        var profile = GetSelectedProfile();
        if (profile is null)
        {
            return;
        }

        txtProfileName.Text = profile.Name;
        numWindowSize.Value = ClampToRange(profile.WindowSize, numWindowSize.Minimum, numWindowSize.Maximum);
        numSeriesLength.Value = ClampToRange(profile.SeriesLength, numSeriesLength.Minimum, numSeriesLength.Maximum);
        numTrainSize.Value = ClampToRange(profile.TrainSize, numTrainSize.Minimum, numTrainSize.Maximum);
        numHorizon.Value = ClampToRange(profile.Horizon, numHorizon.Minimum, numHorizon.Maximum);
        cmbModelType.SelectedValue = profile.ModelType;

        if (!string.IsNullOrWhiteSpace(profile.LocationName))
        {
            txtTrainCity.Text = profile.LocationName;
            txtForecastCity.Text = profile.LocationName;
        }
    }

    private void btnAddProfile_Click(object sender, EventArgs e)
    {
        var name = string.IsNullOrWhiteSpace(txtProfileName.Text)
            ? $"Профиль {DateTime.Now:HHmmss}"
            : txtProfileName.Text.Trim();

        var profile = _profileService.CreateProfile(
            name,
            (int)numWindowSize.Value,
            (int)numSeriesLength.Value,
            (int)numTrainSize.Value,
            (int)numHorizon.Value,
            GetSelectedModelType());

        _profileService.SetActive(profile.Id);
        LoadProfiles();
    }

    private void btnSetActive_Click(object sender, EventArgs e)
    {
        var profile = GetSelectedProfile();
        if (profile is null)
        {
            return;
        }

        _profileService.SetActive(profile.Id);
        LoadProfiles();
    }

    private void btnDeleteProfile_Click(object sender, EventArgs e)
    {
        var profile = GetSelectedProfile();
        if (profile is null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Удалить профиль \"{profile.Name}\" и его модели?",
            "Подтвердить удаление",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        _profileService.DeleteProfile(profile.Id);
        LoadProfiles();
    }

    private void btnSaveProfile_Click(object sender, EventArgs e)
    {
        var profile = GetSelectedProfile();
        if (profile is null)
        {
            return;
        }

        var name = string.IsNullOrWhiteSpace(txtProfileName.Text)
            ? $"Профиль {DateTime.Now:HHmmss}"
            : txtProfileName.Text.Trim();

        profile.Name = name;
        profile.WindowSize = (int)numWindowSize.Value;
        profile.SeriesLength = (int)numSeriesLength.Value;
        profile.TrainSize = (int)numTrainSize.Value;
        profile.Horizon = (int)numHorizon.Value;
        profile.ModelType = GetSelectedModelType();

        _profileService.UpdateProfile(profile);
        LoadProfiles();
    }

    private async void btnTrain_Click(object sender, EventArgs e)
    {
        await RunTrainingAsync(fineTune: false);
    }

    private async void btnFineTune_Click(object sender, EventArgs e)
    {
        await RunTrainingAsync(fineTune: true);
    }

    private async Task RunTrainingAsync(bool fineTune)
    {
        var profile = _profileService.GetActiveProfile();
        if (profile is null)
        {
            lblTrainStatus.Text = "Статус: сначала создайте профиль.";
            return;
        }

        var city = txtTrainCity.Text.Trim();
        if (string.IsNullOrWhiteSpace(city))
        {
            lblTrainStatus.Text = "Статус: укажите город для обучения.";
            return;
        }

        var startUtc = ToUtcDate(dtTrainStart.Value);
        var endUtc = ToUtcDate(dtTrainEnd.Value).AddDays(1).AddHours(-1);
        ToggleTrainingButtons(false);

        try
        {
            var (effectiveStart, effectiveEnd) = GetEffectiveTrainingRange(profile, startUtc, endUtc, fineTune);
            if (!ValidateTrainingWindow(profile, effectiveStart, effectiveEnd))
            {
                return;
            }

            lblTrainStatus.Text = "Статус: загрузка данных...";
            var location = await _apiService.GetLocationAsync(city).ConfigureAwait(true);
            if (location is null)
            {
                lblTrainStatus.Text = "Статус: город не найден.";
                return;
            }

            var data = await _apiService
                .GetHistoricalHourlyAsync(location.Latitude, location.Longitude, effectiveStart, effectiveEnd)
                .ConfigureAwait(true);

            BindTrainingDataGrid(data, effectiveStart, effectiveEnd);

            if (data.Count < profile.WindowSize + profile.Horizon)
            {
                lblTrainStatus.Text = "Статус: недостаточно данных для обучения.";
                MessageBox.Show(
                    "Недостаточно данных для обучения.\nУвеличьте период или уменьшите окно/горизонт.",
                    "Проверка обучения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!ConfirmTrainingStart(profile, effectiveStart, effectiveEnd, data.Count, fineTune))
            {
                lblTrainStatus.Text = "Статус: обучение отменено пользователем.";
                return;
            }

            lblTrainStatus.Text = "Статус: обучение...";
            var result = await _trainingService
                .TrainWithDataAsync(profile, location, effectiveStart, effectiveEnd, data)
                .ConfigureAwait(true);

            lblTrainStatus.Text = $"Статус: {result.Message}";
            LoadProfiles();
        }
        catch (Exception ex)
        {
            lblTrainStatus.Text = $"Статус: ошибка - {ex.Message}";
        }
        finally
        {
            ToggleTrainingButtons(true);
        }
    }

    private bool ValidateTrainingWindow(ModelProfile profile, DateTime startUtc, DateTime endUtc)
    {
        if (endUtc <= startUtc)
        {
            lblTrainStatus.Text = "Статус: неверный диапазон дат.";
            return false;
        }

        var totalHours = (endUtc - startUtc).TotalHours + 1;
        var minRequired = profile.ModelType == ModelType.Boosting
            ? BoostingConstants.LagCount + 1
            : profile.WindowSize * 2 + 1;
        if (totalHours <= minRequired)
        {
            var message =
                "Недостаточно данных для обучения.\n" +
                $"Требуется минимум {minRequired} часов, выбрано ~{(int)totalHours}.\n" +
                "Увеличьте период или уменьшите размер окна.";

            lblTrainStatus.Text = "Статус: недостаточно данных для окна.";
            MessageBox.Show(message, "Проверка обучения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private bool ConfirmTrainingStart(
        ModelProfile profile,
        DateTime startUtc,
        DateTime endUtc,
        int dataCount,
        bool fineTune)
    {
        var totalHours = (endUtc - startUtc).TotalHours + 1;
        var mode = fineTune ? "дообучение" : "обучение";
        var message =
            $"Запустить {mode}?\n" +
            $"Период: {startUtc:yyyy-MM-dd} - {endUtc:yyyy-MM-dd}\n" +
            $"Часов данных: ~{(int)totalHours}, строк: {dataCount}\n" +
            $"Окно: {profile.WindowSize}, горизонт: {profile.Horizon}";

        return MessageBox.Show(
            message,
            "Подтверждение обучения",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private (DateTime startUtc, DateTime endUtc) GetEffectiveTrainingRange(
        ModelProfile profile,
        DateTime startUtc,
        DateTime endUtc,
        bool fineTune)
    {
        if (!fineTune || !profile.LastTrainStartUtc.HasValue || !profile.LastTrainEndUtc.HasValue)
        {
            return (startUtc, endUtc);
        }

        var effectiveStart = startUtc < profile.LastTrainStartUtc.Value
            ? startUtc
            : profile.LastTrainStartUtc.Value;

        var effectiveEnd = endUtc > profile.LastTrainEndUtc.Value
            ? endUtc
            : profile.LastTrainEndUtc.Value;

        return (effectiveStart, effectiveEnd);
    }

    private void BindTrainingDataGrid(
        IReadOnlyList<WeatherDataPoint> data,
        DateTime startUtc,
        DateTime endUtc)
    {
        if (data.Count == 0)
        {
            gridTrainingData.DataSource = null;
            lblTrainingDataInfo.Text = "Данные обучения: нет";
            return;
        }

        var preview = data
            .OrderBy(p => p.Time)
            .Take(200)
            .ToList();

        gridTrainingData.DataSource = preview;
        SetTrainingGridHeaders();

        lblTrainingDataInfo.Text =
            $"Данные обучения: {data.Count} строк, период {startUtc:yyyy-MM-dd} - {endUtc:yyyy-MM-dd} " +
            $"(показано {preview.Count})";
    }

    private void SetTrainingGridHeaders()
    {
        if (gridTrainingData.Columns.Count == 0)
        {
            return;
        }

        gridTrainingData.Columns[nameof(WeatherDataPoint.Time)].HeaderText = "Время (UTC)";
        gridTrainingData.Columns[nameof(WeatherDataPoint.TemperatureC)].HeaderText = "Температура (C)";
        gridTrainingData.Columns[nameof(WeatherDataPoint.Humidity)].HeaderText = "Влажность (%)";
        gridTrainingData.Columns[nameof(WeatherDataPoint.Pressure)].HeaderText = "Давление (hPa)";
        gridTrainingData.Columns[nameof(WeatherDataPoint.WindSpeed)].HeaderText = "Скорость ветра (m/s)";
    }

    private void ToggleTrainingButtons(bool enabled)
    {
        btnTrain.Enabled = enabled;
        btnFineTune.Enabled = enabled;
    }

    private void btnForecast_Click(object sender, EventArgs e)
    {
        var profile = _profileService.GetActiveProfile();
        if (profile is null)
        {
            lblForecastStatus.Text = "Статус: сначала создайте профиль.";
            return;
        }

        var city = txtForecastCity.Text.Trim();
        if (string.IsNullOrWhiteSpace(city))
        {
            lblForecastStatus.Text = "Статус: укажите город для прогноза.";
            return;
        }

        var (startOffsetHours, horizon, requestedHours) = GetForecastWindow(profile);
        if (horizon <= 0)
        {
            lblForecastStatus.Text = "Статус: выберите дату прогноза позже последнего обучения.";
            return;
        }

        if (profile.ModelType == ModelType.Ssa && startOffsetHours + requestedHours > profile.Horizon)
        {
            lblForecastStatus.Text =
                $"Статус: выбранная дата дальше горизонта модели ({profile.Horizon}ч). " +
                "Показан прогноз в пределах горизонта модели.";
        }
        else
        {
            lblForecastStatus.Text = "Статус: прогноз...";
        }

        try
        {
            var points = _forecastService.Forecast(profile, startOffsetHours, horizon);
            gridForecast.DataSource = points.ToList();
            SetForecastGridHeaders();

            var location = profile.LocationName ?? city;
            lblForecastStatus.Text =
                $"Статус: прогноз для {location} (горизонт модели {profile.Horizon}ч, " +
                $"запрос {requestedHours}ч, показано {horizon}ч).";
        }
        catch (Exception ex)
        {
            lblForecastStatus.Text = $"Статус: ошибка - {ex.Message}";
        }
    }

    private (int startOffsetHours, int horizon, int requestedHours) GetForecastWindow(ModelProfile profile)
    {
        var targetStart = dtForecastDate.Value.Date;
        if (!profile.LastTrainEndUtc.HasValue)
        {
            var fallback = (int)numForecastHorizon.Value;
            return (0, fallback, fallback);
        }

        var lastPoint = profile.LastTrainEndUtc.Value.AddHours(1);
        var startOffsetHours = (int)Math.Ceiling((targetStart - lastPoint).TotalHours);
        if (startOffsetHours < 0)
        {
            return (0, 0, (int)numForecastHorizon.Value);
        }

        var requestedHours = (int)numForecastHorizon.Value;
        var horizon = profile.ModelType == ModelType.Boosting
            ? requestedHours
            : Math.Min(requestedHours, profile.Horizon - startOffsetHours);
        return (startOffsetHours, horizon, requestedHours);
    }

    private ModelType GetSelectedModelType()
    {
        if (cmbModelType.SelectedValue is ModelType modelType)
        {
            return modelType;
        }

        return ModelType.Ssa;
    }

    private void SetForecastGridHeaders()
    {
        if (gridForecast.Columns.Count == 0)
        {
            return;
        }

        gridForecast.Columns[nameof(ForecastPoint.Time)].HeaderText = "Время (UTC)";
        gridForecast.Columns[nameof(ForecastPoint.TemperatureC)].HeaderText = "Температура (C)";
        gridForecast.Columns[nameof(ForecastPoint.Humidity)].HeaderText = "Влажность (%)";
        gridForecast.Columns[nameof(ForecastPoint.Pressure)].HeaderText = "Давление (hPa)";
        gridForecast.Columns[nameof(ForecastPoint.WindSpeed)].HeaderText = "Скорость ветра (m/s)";
    }

    private static DateTime ToUtcDate(DateTime localDate)
    {
        var local = DateTime.SpecifyKind(localDate.Date, DateTimeKind.Local);
        return local.ToUniversalTime();
    }

    private static decimal ClampToRange(int value, decimal min, decimal max)
    {
        if (value < (int)min) return min;
        if (value > (int)max) return max;
        return value;
    }
}
