namespace WeatherApp.Core.Models;

public static class BoostingConstants
{
    public const int LagCount = 24;
    public const int TimeFeatureCount = 6;
    public const int FeatureCount = LagCount + TimeFeatureCount;
}
