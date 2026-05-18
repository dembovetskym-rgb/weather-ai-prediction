namespace WeatherApp.Core.Models;

public sealed class WeatherDataPoint
{
    public DateTime Time { get; set; }
    public float TemperatureC { get; set; }
    public float Humidity { get; set; }
    public float Pressure { get; set; }
    public float WindSpeed { get; set; }
}
