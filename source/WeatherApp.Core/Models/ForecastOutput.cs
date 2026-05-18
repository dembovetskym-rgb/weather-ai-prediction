namespace WeatherApp.Core.Models;

public sealed class ForecastOutput
{
    public float[] Forecasted { get; set; } = Array.Empty<float>();
    public float[] LowerBound { get; set; } = Array.Empty<float>();
    public float[] UpperBound { get; set; } = Array.Empty<float>();
}
