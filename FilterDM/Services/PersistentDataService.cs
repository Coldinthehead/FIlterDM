using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class UserPrefrences
{
    public Dictionary<string, string> Preferences { get; set; }
}
public class PersistentDataService : IPersistentDataService
{
    private UserPrefrences _prefs;

    public string BaseReporsitoryPath { get; private set; }
    public string FiltersPath { get; private set; }
    public string TemplatesPath { get; private set; }

    public string TempaltesFolderName { get; private set; } = "templates";
    public string FiltersFolderName { get; private set; } = "filters";
    public string PreferenceFolderName { get; private set; } = "preferences";
    public string PreferenceFileName { get; private set; } = "preferences.json";
    public string StorageFolderName { get; private set; } = "FilterDM";
    public string? GetPreference(string key)
    {
        if (_prefs.Preferences.TryGetValue(key, out string? value))
        {
            return value;
        }
        return null;
    }

    public void SetPreference(string key, string value)
    {
        _prefs.Preferences[key] = value;
    }

    public async Task InitFolders()
    {
        BaseReporsitoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), StorageFolderName);
        FiltersPath = Path.Combine(BaseReporsitoryPath, FiltersFolderName);
        TemplatesPath = Path.Combine(BaseReporsitoryPath, TempaltesFolderName);
        
        if (!Directory.Exists(BaseReporsitoryPath))
        {
            DirectoryInfo info = Directory.CreateDirectory(BaseReporsitoryPath);
            info.CreateSubdirectory(FiltersFolderName);
            info.CreateSubdirectory(TempaltesFolderName);
            info.CreateSubdirectory(PreferenceFolderName);
            UserPrefrences empty = new();
            using var fs = File.Create(Path.Combine(info.FullName, PreferenceFolderName, PreferenceFileName));
            await JsonSerializer.SerializeAsync(fs, empty);
        }
        string prefsPath = Path.Combine(BaseReporsitoryPath, PreferenceFolderName, PreferenceFileName);
        using var readStream = File.Open(prefsPath, FileMode.Open);
        _prefs = await JsonSerializer.DeserializeAsync<UserPrefrences>(readStream);
        if (_prefs == null)
        {
            throw new FileNotFoundException($"Cannot found play preferences file at '{prefsPath}'");
        }
    }
}
