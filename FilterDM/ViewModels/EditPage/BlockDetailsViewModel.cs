using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage;

public partial class BlockDetailsViewModel : ObservableRecipient
    , IRecipient<RuleDeleteRequestEvent>
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
    private ObservableCollection<string> _templates;

    [ObservableProperty]
    private string _selectedTempalte;

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
        if (SelectedTempalte != null)
        {
            if (Rules.Count > 0)
            {
                var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to override {Rules.Count} rules?");
                if (!confirm)
                {
                    return;
                }
            }
            var service = App.Current.Services.GetService<BlockTemplateService>();
            BlockModel? nextTeplate = service.GetTemplate(SelectedTempalte);
            if (nextTeplate != null)
            {
                nextTeplate.Title = Title;
                SetModel(nextTeplate);

            }
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
                Messenger.Send(new DeleteBlockRequest(this));
                Messenger.Send(new FilterEditedRequestEvent(this));
            }
        }
        else
        {
            Messenger.Send(new DeleteBlockRequest(this));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [RelayCommand]
    public void NewRule()
    {
        var teplateSerivice = App.Current.Services.GetService<RuleTemplateService>();
        var model = teplateSerivice.BuildEmpty();
        var vm = AddRule(model);
        Messenger.Send(new RuleCreateRequestEvent(vm));
    }

    public float CalculatedPriority => (Enabled ? -1 : 1) * Priority;

    private readonly ObservableCollection<BlockDetailsViewModel> _allBlocks;
    public BlockDetailsViewModel(ObservableCollection<BlockDetailsViewModel> allBlocks
        , ObservableCollection<string> templateNames
        , TypeScopeManager scopeManager
        )
    {
        _allBlocks = allBlocks;
        Messenger.Register<RuleDeleteRequestEvent>(this);

        Templates = templateNames;
        _scopeManager = scopeManager;
    }

    public BlockModel GetModel()
    {
        BlockModel block = App.Current.Services.GetService<BlockTemplateService>().GetEmpty();
        block.Title = Title;
        block.Enabled = Enabled;
        block.Priority = Priority;
        block.TemplateName = SelectedTempalte;
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

        Rules.Clear();
        foreach (var rule in currentRules)
        {
            rule.DeleteSafe();
        }
        UseScopeNames = model.UseBlockTypeScope;
        foreach (var rule in model.Rules)
        {
            AddRule(rule);
        }
        Title = model.Title;
        Enabled = model.Enabled;
        Priority = model.Priority;
      
        if (model.TemplateName != null && Templates.Contains(model.TemplateName))
        {
            SelectedTempalte = model.TemplateName;
        }
        else
        {
            SelectedTempalte = "Empty";
        }
    }

    public RuleDetailsViewModel AddRule(RuleModel model)
    {
        var ruleVm = new RuleDetailsViewModel(_allBlocks, this, new());
        model.Title = GetNextTitle(model.Title);
        ruleVm.SetModel(model);
        Rules.Add(ruleVm);
        return ruleVm;
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
        Rules.Add(rule);
        _scopeManager.AddByExistingRule(rule);
        SortRules();
    }

    public void SortRules()
    {
        List<RuleDetailsViewModel> sorted = [.. Rules.OrderBy(x => x.CalculatedPriority)];
        Rules = new ObservableCollection<RuleDetailsViewModel>(sorted);
    }

    public void Receive(RuleDeleteRequestEvent message)
    {
        Rules.Remove(message.Value);
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
