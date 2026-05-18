using Microsoft.ML.Data;

namespace WeatherApp.Core.Models;

public sealed class BoostingInput
{
    [VectorType(BoostingConstants.FeatureCount)]
    public float[] Features { get; set; } = Array.Empty<float>();

    public float Label { get; set; }
}
