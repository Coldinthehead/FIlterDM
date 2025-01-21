using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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

    [ObservableProperty]
    private BlockDetailsViewModel _selectedParent;

    [ObservableProperty]
    private RuleModel _selectedTemplate;

    [RelayCommand]
    private void ApplyProperties()
    {
        if (ParentManager.RequireChange(Rule, SelectedParent))
        {
            ParentManager.ChangeParent(Rule, SelectedParent);
        }
        else
        {
            OnSortRules();
        }
        Messenger.Send(new RuleTitleApplyEvent(Rule), Rule);
    }

    [RelayCommand]
    private async Task Reset()
    {
        if (Rule.Modifiers.Count > 2)
        {
            bool confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to override Rule with {Rule.Modifiers.Count} modifiers?");
            if (confirm)
            {
                OnResetConfirmed();
            }
        }
        OnResetConfirmed();
    }

    public void OnSortRules()
    {
        Messenger.Send(new SortRulesRequest(Rule));
    }

    public void OnResetConfirmed()
    {
        TemplateManager.SetTemplate(Rule, SelectedTemplate);
    }

    [ObservableProperty]
    private RuleParentManager _parentManager;

    [ObservableProperty]
    private RuleTemplateManager _templateManager;

    public RulePropertiesDecoratorViewModel(RuleDetailsViewModel rule
        , RuleParentManager parentManager
        , RuleTemplateManager templateManager) : base(rule, null)
    {

        ParentManager = parentManager;
        TemplateManager = templateManager;
    }

    public override void Apply(RuleModel model)
    {
        model.Title = Title;
        model.Enabled = Enabled;
        model.Priority = Priority;
        model.Show = Show;
        model.TemplateName = SelectedTemplate.Title;
    }

    internal void SetModel(RuleModel rule)
    {
        Title = rule.Title;
        Enabled = rule.Enabled;
        Priority = rule.Priority;
        Show = rule.Show;
        SelectedTemplate = TemplateManager.Get(rule.TemplateName);
    }

    public override ModifierEditorViewModel GetEditor() => new RulePropertiesEditorViewModel(Rule, this);
}
