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
