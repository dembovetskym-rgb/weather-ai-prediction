using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services;

public sealed class DatasetBuilder
{
    public Dictionary<MetricType, List<float>> BuildSeries(IReadOnlyList<WeatherDataPoint> points)
    {
        var ordered = points.OrderBy(p => p.Time).ToList();

        return new Dictionary<MetricType, List<float>>
        {
            [MetricType.TemperatureC] = ordered.Select(p => p.TemperatureC).ToList(),
            [MetricType.Humidity] = ordered.Select(p => p.Humidity).ToList(),
            [MetricType.Pressure] = ordered.Select(p => p.Pressure).ToList(),
            [MetricType.WindSpeed] = ordered.Select(p => p.WindSpeed).ToList()
        };
    }
}
