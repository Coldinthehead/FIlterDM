using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage;

public partial class BlockDetailsViewModel : ObservableRecipient
    ,IRecipient<RuleDeleteRequestEvent>
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private bool _isSelected;

    partial void OnTitleChanged(string? oldValue, string newValue)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private bool _enabled;
    partial void OnEnabledChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [ObservableProperty]
    private float _priority;
    partial void OnPriorityChanged(float oldValue, float newValue)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private RuleDetailsViewModel _selectedRule;


    [RelayCommand]
    public async void DeleteBlock()
    {
        if (Rules.Count > 0)
        {
            var dialogResult = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete {Rules.Count} rules?");
            if (dialogResult)
            {
                Messenger.Send(new BlockDeleteRequestEvent(this));
                Messenger.Send(new FilterEditedRequestEvent(this));
            }
        }
        else
        {
            Messenger.Send(new BlockDeleteRequestEvent(this));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [ObservableProperty]
    private ObservableCollection<RuleDetailsViewModel> _rules = new();

    [RelayCommand]
    public void NewRule()
    {
        var teplateSerivice = App.Current.Services.GetService<RuleTemplateService>();
        var model = teplateSerivice.BuildEmpty();
        var vm =  AddRule(model);
        Messenger.Send(new RuleCreateRequestEvent(vm));
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

    public float CalculatedPriority => (Enabled ? -1 : 1) * Priority;

    private readonly ObservableCollection<BlockDetailsViewModel> _allBlocks;
    public BlockDetailsViewModel(ObservableCollection<BlockDetailsViewModel> allBlocks)
    {
        _allBlocks = allBlocks;
        Messenger.Register<RuleDeleteRequestEvent>(this);
    }

    public void SetModel(BlockModel model)
    {
        foreach (var rule in Rules)
        {
            rule.DeleteMeCommand.Execute(null);
        }

        Rules.Clear();
        foreach (var  rule in model.Rules)
        {
            AddRule(rule);
        }
        Title = model.Title;
        Enabled = model.Enabled;
        Priority = model.Priority;
    }

    public RuleDetailsViewModel AddRule(RuleModel model)
    {
        var ruleVm = new RuleDetailsViewModel(_allBlocks, this);
        model.Title = GetNextTitle(model.Title);
        ruleVm.SetModel(model);
        Rules.Add(ruleVm);
        return ruleVm;
    }

    public bool DeleteRule(RuleDetailsViewModel rule)
    {
        if (Rules.Contains(rule))
        {
            Rules.Remove(rule);
            return true;
        }
        return false;
    }

    public void AddRule(RuleDetailsViewModel rule)
    {
        rule.Properties.Title = GetNextTitle(rule.Properties.Title);
        Rules.Add(rule);
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
}
