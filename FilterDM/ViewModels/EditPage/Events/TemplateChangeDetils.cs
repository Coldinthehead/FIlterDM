using FilterDM.Models;

namespace FilterDM.ViewModels.EditPage.Events;

public struct TemplateChangeDetils
{
    public readonly BlockDetailsViewModel Block;
    public readonly BlockModel Template;

    public TemplateChangeDetils(BlockDetailsViewModel block, BlockModel template)
    {
        Block = block;
        Template = template;
    }
}

