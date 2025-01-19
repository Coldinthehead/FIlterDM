
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class RuleDetailsViewModelTests
{
    [Test]
    public void OnDeleteConfirmed_ShouldRaiseRuleDeleteRequest()
    {
        RuleDetailsViewModel sut = new(new([]), null, []);
        EventListener<DeleteRuleRequest, RuleDetailsViewModel> listener = new();
        sut.OnDeleteConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(sut));
    }
}
