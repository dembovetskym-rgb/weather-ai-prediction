namespace WeatherApp.Core.Models;

public sealed class BoostingHistory
{
    public DateTime LastTimeUtc { get; set; }
    public float[] LastValues { get; set; } = Array.Empty<float>();
    public float MinValue { get; set; }
    public float MaxValue { get; set; }
}
