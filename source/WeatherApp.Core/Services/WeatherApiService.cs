using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services;

public sealed class WeatherApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public WeatherApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<GeoLocation?> GetLocationAsync(string city, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return null;
        }

        var url =
            $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json";

        using var response = await _http.GetAsync(url, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var payload = JsonSerializer.Deserialize<GeoResponse>(json, _jsonOptions);
        var result = payload?.Results?.FirstOrDefault();
        if (result is null)
        {
            return null;
        }

        return new GeoLocation
        {
            Name = result.Name ?? city,
            Latitude = result.Latitude,
            Longitude = result.Longitude,
            Country = result.Country
        };
    }

    public async Task<IReadOnlyList<WeatherDataPoint>> GetHistoricalHourlyAsync(
        double latitude,
        double longitude,
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken ct = default)
    {
        var startDate = startUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var endDate = endUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var url =
            "https://archive-api.open-meteo.com/v1/archive" +
            $"?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
            $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
            $"&start_date={startDate}&end_date={endDate}" +
            "&hourly=temperature_2m,relative_humidity_2m,pressure_msl,wind_speed_10m" +
            "&timezone=UTC";

        using var response = await _http.GetAsync(url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var payload = JsonSerializer.Deserialize<ArchiveResponse>(json, _jsonOptions);
        var hourly = payload?.Hourly;
        if (hourly?.Time is null || hourly.Time.Length == 0)
        {
            return Array.Empty<WeatherDataPoint>();
        }

        var count = hourly.Time.Length;
        var points = new List<WeatherDataPoint>(count);
        for (var i = 0; i < count; i++)
        {
            if (!DateTime.TryParse(hourly.Time[i], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var time))
            {
                continue;
            }

            points.Add(new WeatherDataPoint
            {
                Time = time.ToUniversalTime(),
                TemperatureC = hourly.Temperature2m?[i] ?? 0f,
                Humidity = hourly.RelativeHumidity2m?[i] ?? 0f,
                Pressure = hourly.PressureMsl?[i] ?? 0f,
                WindSpeed = hourly.WindSpeed10m?[i] ?? 0f
            });
        }

        return points;
    }

    public sealed class GeoLocation
    {
        public string Name { get; set; } = string.Empty;
        public string? Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    private sealed class GeoResponse
    {
        [JsonPropertyName("results")]
        public GeoResult[]? Results { get; set; }
    }

    private sealed class GeoResult
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    private sealed class ArchiveResponse
    {
        [JsonPropertyName("hourly")]
        public HourlyData? Hourly { get; set; }
    }

    private sealed class HourlyData
    {
        [JsonPropertyName("time")]
        public string[]? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public float[]? Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public float[]? RelativeHumidity2m { get; set; }

        [JsonPropertyName("pressure_msl")]
        public float[]? PressureMsl { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public float[]? WindSpeed10m { get; set; }
    }
}
