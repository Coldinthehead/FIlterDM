using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Events;

public class BlockInFilterCreated : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockInFilterCreated(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class BlockCollectionInFilterChanged : ValueChangedMessage<ObservableCollection<BlockDetailsViewModel>>
{
    public BlockCollectionInFilterChanged(ObservableCollection<BlockDetailsViewModel> value) : base(value)
    {
    }
}

public class BlockDeletedInFilter : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockDeletedInFilter(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class RuleCreatedInFilter : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleCreatedInFilter(RuleDetailsViewModel value) : base(value)
    {
    }
}

public class BlockSelectedInTree : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockSelectedInTree(BlockDetailsViewModel value) : base(value)
    {
    }
}


public class BlockEditorClosed : ValueChangedMessage<BlockEditorViewModel>
{
    public BlockEditorClosed(BlockEditorViewModel value) : base(value)
    {
    }
}
