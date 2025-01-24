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

public class PersistentDataService
{
    private UserPrefrences _prefs;

    public string BaseReporsitoryPath { get; private set; }
    public string FiltersPath { get; private set; }
    public string TemplatesPath { get; private set; }

    public const string TempaltesFolderName = "templates";
    public const string FiltersFolderName = "filters";
    public const string PreferenceFolderName = "preferences";
    public const string PreferenceFileName = "preferences.json";
    public const string StorageFolderName = "FilterDM";
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
