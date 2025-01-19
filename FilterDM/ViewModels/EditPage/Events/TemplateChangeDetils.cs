namespace FilterDM.ViewModels.EditPage.Events;

public struct TemplateChangeDetils
{
    public readonly BlockDetailsViewModel Block;
    public readonly string TempalteName;

    public TemplateChangeDetils(BlockDetailsViewModel block, string tempalteName)
    {
        Block = block;
        TempalteName = tempalteName;
    }
}

