﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;


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
    private ObservableCollection<string> _templates;

    [ObservableProperty]
    private string _selectedTemplate;

    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _allBlocks;

    [ObservableProperty]
    private BlockDetailsViewModel _selectedParent;

    public BlockDetailsViewModel RealParent => _realParent;
    private BlockDetailsViewModel _realParent;

    [RelayCommand]
    private void ApplyProperties()
    {
        if (SelectedParent == null || _realParent == null)
        {
            return;
        }

        if (RealParent == SelectedParent)
        {
            OnSortRules();
        }
        else
        {
            RealParent.DeleteRule(Rule);
            _realParent = SelectedParent;
            SelectedParent.AddRule(Rule);
        }

        Messenger.Send(new RuleTitleApplyEvent(Rule), Rule);
    }

    [RelayCommand]
    private void Reset()
    {
        if (SelectedTemplate != null)
        {
            RuleTemplateService? service = App.Current.Services.GetService<RuleTemplateService>();
            RuleModel? nextTempate = service.GetTemplate(SelectedTemplate);
            if (nextTempate != null)
            {
                var title = Title;
                Rule.SetModel(nextTempate);
                Title = title;
            }
        }
    }

    public void OnSortRules()
    {
        Messenger.Send(new SortRulesRequest(Rule));
    }

    public RulePropertiesDecoratorViewModel(RuleDetailsViewModel rule
        , ObservableCollection<BlockDetailsViewModel> allBlocks
        , BlockDetailsViewModel parentBlock
        , ObservableCollection<string> templates) : base(rule, null)
    {

        Templates = templates;
        AllBlocks = allBlocks;
        SelectedParent = parentBlock;
        _realParent = parentBlock;
    }

    public override void Apply(RuleModel model)
    {
        model.Title = Title;
        model.Enabled = Enabled;
        model.Priority = Priority;
        model.Show = Show;
        model.TemplateName = SelectedTemplate;
    }

    internal void SetModel(RuleModel rule)
    {
        Title = rule.Title;
        Enabled = rule.Enabled;
        Priority = rule.Priority;
        Show = rule.Show;

        if (rule.TemplateName != null && Templates.Contains(rule.TemplateName))
        {
            SelectedTemplate = rule.TemplateName;
        }
        else
        {
            SelectedTemplate = "Empty";
        }
    }
}
