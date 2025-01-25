namespace FilterDM.Services;

public interface IPersistentDataService
{
    public string FiltersPath { get; }
    public string RuleTemaplatesPath { get; }
    public string BlockTemaplatesPath { get; }
    public string FilterTemaplatesPath { get; }
}
