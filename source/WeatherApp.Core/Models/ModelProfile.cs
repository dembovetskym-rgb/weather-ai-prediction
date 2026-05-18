namespace WeatherApp.Core.Models;

public sealed class ModelProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "Новый профиль";

    public int WindowSize { get; set; } = 24;
    public int SeriesLength { get; set; } = 48;
    public int TrainSize { get; set; } = 0;
    public int Horizon { get; set; } = 24;
    public ModelType ModelType { get; set; } = ModelType.Ssa;

    public string? LocationName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastTrainStartUtc { get; set; }
    public DateTime? LastTrainEndUtc { get; set; }
}
