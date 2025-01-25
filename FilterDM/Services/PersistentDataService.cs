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

    public string RuleTemaplatesPath { get; private set; }
    public string BlockTemaplatesPath { get; private set; }
    public string FilterTemaplatesPath { get; private set; }

    public const string TEMPLATES_FOLDER_NAME  = "templates";
    public const string FILTERS_FOLDER_NAME  = "filters";
    public const string PREFERENCES_FOLDER_NAME  = "preferences";
    public const string PREFERENCES_FILE_NAME = "preferences.json";
    public const string STORAGE_FOLDER_NAME  = "FilterDM";

    private const string RULE_TEMPLATES_FOLDER_NAME = "rule_templates";
    private const string BLOCK_TEMPLATES_FOLDER_NAME = "block_templates";
    private const string FILTER_TEMPLATES_FOLDER_NAME = "filter_templates";
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
        BaseReporsitoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), STORAGE_FOLDER_NAME);
        FiltersPath = Path.Combine(BaseReporsitoryPath, FILTERS_FOLDER_NAME);
        TemplatesPath = Path.Combine(BaseReporsitoryPath, TEMPLATES_FOLDER_NAME);
        RuleTemaplatesPath = Path.Combine(TemplatesPath, RULE_TEMPLATES_FOLDER_NAME);
        BlockTemaplatesPath = Path.Combine(TemplatesPath, BLOCK_TEMPLATES_FOLDER_NAME);
        FilterTemaplatesPath = Path.Combine(TemplatesPath, FILTER_TEMPLATES_FOLDER_NAME);

        if (!Directory.Exists(BaseReporsitoryPath))
        {
            DirectoryInfo info = Directory.CreateDirectory(BaseReporsitoryPath);
            info.CreateSubdirectory(FILTERS_FOLDER_NAME);

            DirectoryInfo templatesFolder = info.CreateSubdirectory(TEMPLATES_FOLDER_NAME);
           
            templatesFolder.CreateSubdirectory(RULE_TEMPLATES_FOLDER_NAME);
          
            templatesFolder.CreateSubdirectory(BLOCK_TEMPLATES_FOLDER_NAME);
            templatesFolder.CreateSubdirectory(FILTER_TEMPLATES_FOLDER_NAME);

            info.CreateSubdirectory(PREFERENCES_FOLDER_NAME);
            UserPrefrences empty = new();
            using var fs = File.Create(Path.Combine(info.FullName, PREFERENCES_FOLDER_NAME, PREFERENCES_FILE_NAME));
            await JsonSerializer.SerializeAsync(fs, empty);
        }
        string prefsPath = Path.Combine(BaseReporsitoryPath, PREFERENCES_FOLDER_NAME, PREFERENCES_FILE_NAME);
        using var readStream = File.Open(prefsPath, FileMode.Open);
        _prefs = await JsonSerializer.DeserializeAsync<UserPrefrences>(readStream);
        if (_prefs == null)
        {
            throw new FileNotFoundException($"Cannot found play preferences file at '{prefsPath}'");
        }
    }
}
