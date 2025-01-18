using FilterDM.Models;


namespace FilterDM.ViewModels.EditPage.Events;

public readonly struct BlockModelChangedDetails
{
    public readonly BlockModel Old;
    public readonly BlockModel New;

    public BlockModelChangedDetails(BlockModel old, BlockModel @new)
    {
        Old = old;
        New = @new;
    }
}


