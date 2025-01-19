using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockDetailsViewModelTests
{
    [Test]
    public void NewRuleCommand_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(), new(), new(new()));
        RuleCreateRequestListener listener = new();

        sut.NewRuleCommand.Execute(sut);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Block, Is.EqualTo(sut));
    }


    public class RuleCreateRequestListener : ObservableRecipient, IRecipient<CreateRuleRequest>
    {
        public bool Received = false;
        public BlockDetailsViewModel Block;

        public RuleCreateRequestListener()
        {
            Messenger.Register(this);
        }
        public void Receive(CreateRuleRequest message)
        {
            Received = true;
            Block = message.Value;
        }
    }

}
