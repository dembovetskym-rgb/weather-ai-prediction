namespace WeatherApp.Core.Models;

public sealed class ModelProfileStore
{
    public string? ActiveProfileId { get; set; }
    public List<ModelProfile> Profiles { get; set; } = new();
}
