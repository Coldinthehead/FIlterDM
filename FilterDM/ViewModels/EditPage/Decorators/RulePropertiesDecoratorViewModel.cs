using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;


namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class RulePropertiesDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private string _title;
    partial void OnTitleChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private bool _enabled;
    partial void OnEnabledChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private float _priority;
    partial void OnPriorityChanged(float value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private bool _show;
    partial void OnShowChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    internal void SetModel(RuleModel rule)
    {
        Title = rule.Title;
        Enabled = rule.Enabled;
        Priority = rule.Priority;
        Show = rule.Show;
    }

    public RulePropertiesDecoratorViewModel(RuleDetailsViewModel rule) : base(rule, null)
    {
    }
}
