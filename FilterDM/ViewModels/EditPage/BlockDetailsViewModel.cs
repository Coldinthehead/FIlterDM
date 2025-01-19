using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage;

public partial class BlockDetailsViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<RuleDetailsViewModel> _rules = new();

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private bool _isSelected;

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
    private RuleDetailsViewModel _selectedRule;

    [ObservableProperty]
    private BlockModel _selectedTemplate;

    private readonly TypeScopeManager _scopeManager;
    [ObservableProperty]
    private bool _useScopeNames;
    partial void OnUseScopeNamesChanged(bool oldValue, bool newValue)
    {
        if (newValue == false)
        {
            _scopeManager.DisableScope();

        }
        else if (newValue == true)
        {
            _scopeManager.EnableScope();
        }
    }

    [RelayCommand]
    private async void Reset()
    {
        if (SelectedTemplate != null)
        {
            if (Rules.Count > 0)
            {
                var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to override {Rules.Count} rules?");
                if (!confirm)
                {
                    return;
                }
            }
            OnTemplateResetConfirmed();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
        Messenger.Send(new BlockPriorityChangedRequest(this));
    }


    [RelayCommand]
    public async void DeleteBlock()
    {
        if (Rules.Count > 0)
        {
            var dialogResult = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete {Rules.Count} rules?");
            if (dialogResult)
            {
                OnDeleteConfirmed();
            }
        }
        else
        {
            OnDeleteConfirmed();
        }
    }

    [RelayCommand]
    public void NewRule()
    {
        Messenger.Send(new CreateRuleRequest(this));
    }

    public void OnDeleteConfirmed()
    {
        Messenger.Send(new DeleteBlockRequest(this));
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    public void OnTemplateResetConfirmed()
    {
        TemplateManager.SetTempalte(this, SelectedTemplate);
    }

    public float CalculatedPriority => (Enabled ? -1 : 1) * Priority;

    public TypeScopeManager ScopeManager => _scopeManager;

    [ObservableProperty]
    public BlockTemplateManager _templateManager;

    public BlockDetailsViewModel(BlockTemplateManager templateManager , TypeScopeManager scopeManager )
    {
        TemplateManager = templateManager;
        _scopeManager = scopeManager;
    }

    public BlockModel GetModel()
    {
        BlockModel block = App.Current.Services.GetService<BlockTemplateRepository>().GetEmpty();
        block.Title = Title;
        block.Enabled = Enabled;
        block.Priority = Priority;
        block.TemplateName = SelectedTemplate.Title;
        block.UseBlockTypeScope = UseScopeNames;
        foreach (RuleDetailsViewModel rule in Rules)
        {
            block.AddRule(rule.GetModel());
        }
        return block;
    }

    public void SetModel(BlockModel model)
    {
        var currentRules = Rules.ToArray();
        UseScopeNames = model.UseBlockTypeScope;
        Title = model.Title;
        Enabled = model.Enabled;
        Priority = model.Priority;
        SelectedTemplate = TemplateManager.GetTemplate(model.TemplateName);
    }
    public bool DeleteRule(RuleDetailsViewModel rule)
    {
        if (Rules.Remove(rule))
        {
            if (UseScopeNames)
            {
                _scopeManager.RemoveByRule(rule);
            }
            return true;
        }
        return false;
    }

    public void AddRule(RuleDetailsViewModel rule)
    {
        rule.Properties.Title = GetNextTitle(rule.Properties.Title);
        rule.Properties.SelectedParent = this;
        Rules.Add(rule);
        _scopeManager.AddByExistingRule(rule);
        SortRules();
    }

    public void SortRules()
    {
        List<RuleDetailsViewModel> sorted = [.. Rules.OrderBy(x => x.CalculatedPriority)];
        Rules = new ObservableCollection<RuleDetailsViewModel>(sorted);
    }

    private string GetNextTitle(string title)
    {
        int i = 0;
        string res = title;
        List<string> titles = Rules.Select(x => x.Properties.Title).ToList();
        while (true)
        {

            if (titles.Contains(res))
            {
                res = $"{title}({i++})";
            }
            else
            {
                return res;
            }
        }
    }

    internal TypeDecoratorViewModel GetTypeDecorator(RuleDetailsViewModel vm)
    {
        return _scopeManager.GetDecorator(vm);
    }
}
