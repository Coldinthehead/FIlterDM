namespace FilterDM.Services;

public interface IPersistentDataService
{
    public string BaseReporsitoryPath { get; }
    public string FiltersPath { get; }
    public string TemplatesPath { get; }

    public string TempaltesFolderName { get; }
    public string FiltersFolderName { get; }
    public string PreferenceFolderName { get; }
    public string PreferenceFileName { get; }
    public string StorageFolderName { get; }
}
