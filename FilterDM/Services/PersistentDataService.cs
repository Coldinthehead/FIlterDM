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
        string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FilterDM");
        if (Directory.Exists(basePath))
        {
            DirectoryInfo info = Directory.CreateDirectory(basePath);
            info.CreateSubdirectory("filters");
            info.CreateSubdirectory("templates");
            info.CreateSubdirectory("prefrences");
            UserPrefrences empty = new();
            using var fs = File.Create(Path.Combine(info.FullName, "prefrences", "prefrences.json"));
            await JsonSerializer.SerializeAsync(fs, empty);
        }
        string prefsPath = Path.Combine(basePath, "prefrences", "prefrences.json");
        using var readStream = File.Open(prefsPath, FileMode.Open);
        _prefs = await JsonSerializer.DeserializeAsync<UserPrefrences>(readStream);
        if (_prefs == null)
        {
            throw new FileNotFoundException($"Cannot found play preferences file at '{prefsPath}'");
        }

    }
}
