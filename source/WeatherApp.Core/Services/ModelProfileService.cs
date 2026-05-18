using System.Text.Json;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services;

public sealed class ModelProfileService
{
    private readonly string _rootPath;
    private readonly string _profilesFilePath;

    public ModelProfileService(string rootPath)
    {
        _rootPath = rootPath;
        Directory.CreateDirectory(_rootPath);
        _profilesFilePath = Path.Combine(_rootPath, "profiles.json");
    }

    public ModelProfileStore LoadStore()
    {
        if (!File.Exists(_profilesFilePath))
        {
            return new ModelProfileStore();
        }

        var json = File.ReadAllText(_profilesFilePath);
        return JsonSerializer.Deserialize<ModelProfileStore>(json) ?? new ModelProfileStore();
    }

    public void SaveStore(ModelProfileStore store)
    {
        Directory.CreateDirectory(_rootPath);
        var json = JsonSerializer.Serialize(store, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_profilesFilePath, json);
    }

    public ModelProfile CreateProfile(
        string name,
        int windowSize,
        int seriesLength,
        int trainSize,
        int horizon,
        ModelType modelType)
    {
        var store = LoadStore();

        var profile = new ModelProfile
        {
            Name = name,
            WindowSize = windowSize,
            SeriesLength = seriesLength,
            TrainSize = trainSize,
            Horizon = horizon,
            ModelType = modelType,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow
        };

        store.Profiles.Add(profile);
        store.ActiveProfileId ??= profile.Id;
        SaveStore(store);

        return profile;
    }

    public void DeleteProfile(string profileId)
    {
        var store = LoadStore();
        var profile = store.Profiles.FirstOrDefault(p => p.Id == profileId);
        if (profile is null)
        {
            return;
        }

        store.Profiles.Remove(profile);
        if (store.ActiveProfileId == profileId)
        {
            store.ActiveProfileId = store.Profiles.FirstOrDefault()?.Id;
        }

        SaveStore(store);

        var modelDir = Path.Combine(_rootPath, "models", profileId);
        if (Directory.Exists(modelDir))
        {
            Directory.Delete(modelDir, recursive: true);
        }
    }

    public void SetActive(string profileId)
    {
        var store = LoadStore();
        if (store.Profiles.All(p => p.Id != profileId))
        {
            return;
        }

        store.ActiveProfileId = profileId;
        SaveStore(store);
    }

    public ModelProfile? GetActiveProfile()
    {
        var store = LoadStore();
        return store.Profiles.FirstOrDefault(p => p.Id == store.ActiveProfileId);
    }

    public IReadOnlyList<ModelProfile> GetProfiles()
    {
        return LoadStore().Profiles;
    }

    public void UpdateProfile(ModelProfile profile)
    {
        var store = LoadStore();
        var existing = store.Profiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existing is null)
        {
            return;
        }

        profile.UpdatedUtc = DateTime.UtcNow;
        store.Profiles.Remove(existing);
        store.Profiles.Add(profile);
        SaveStore(store);
    }

    public string GetProfileModelDirectory(string profileId)
    {
        var dir = Path.Combine(_rootPath, "models", profileId);
        Directory.CreateDirectory(dir);
        return dir;
    }

    public string GetModelPath(string profileId, MetricType metric)
    {
        var dir = GetProfileModelDirectory(profileId);
        return Path.Combine(dir, $"{metric}.zip");
    }

    public string GetBoostingHistoryPath(string profileId, MetricType metric)
    {
        var dir = GetProfileModelDirectory(profileId);
        return Path.Combine(dir, $"{metric}.history.json");
    }
}
