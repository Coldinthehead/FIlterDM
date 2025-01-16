using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
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
        _model.Title = newValue;
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private bool _enabled;
    partial void OnEnabledChanged(bool oldValue, bool newValue)
    {
        _model.Enabled = newValue;
        if (oldValue != newValue)
        {
            Messenger.Send(new BlockEnabledChangedEvent(_model));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [ObservableProperty]
    private float _priority;
    partial void OnPriorityChanged(float oldValue, float newValue)
    {
        _model.Priority = newValue;
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
        var templateService = App.Current.Services.GetService<RuleTemplateService>();
        RuleModel rule = templateService.BuildEmpty();
        rule.Title = _model.GetGenericRuleTitle();
        _model.AddRule(rule);
        var newvm = new RuleDetailsViewModel(rule, _allBlocks, this);
        Rules.Add(newvm);
        Messenger.Send(new RuleCreateRequestEvent(newvm));

    }

    public float CalculatedPriority => (_model.Enabled ? -1 : 1) * _model.Priority;
    public BlockModel Model => _model;


    private BlockModel _model;

    private readonly ObservableCollection<BlockDetailsViewModel> _allBlocks;
    public BlockDetailsViewModel(BlockModel model, ObservableCollection<BlockDetailsViewModel> allBlocks)
    {
        _allBlocks = allBlocks;
        SetBlocks(model);
        Messenger.Register<RuleDeleteRequestEvent>(this);
    }

    public void SetBlocks(BlockModel model)
    {
        if (_model != null && _model != model)
        {
            var old = Rules.ToArray();
            Rules = new ObservableCollection<RuleDetailsViewModel>();
            foreach (var r in old)
            {
                r.DeleteMeCommand.Execute(null);
            }
        }
        _model = model;
        Title = model.Title;
        Enabled = model.Enabled;
        Priority = model.Priority;
        Rules = new ObservableCollection<RuleDetailsViewModel>();
        foreach (var r in _model.Rules)
        {
            var vm = new RuleDetailsViewModel(r, _allBlocks, this);
            AddRule(vm);
        }
        
       
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public bool DeleteRule(RuleDetailsViewModel rule)
    {
        if (Rules.Contains(rule))
        {
           /* var model = rule.Model;
            Model.DeleteRule(model);*/
            Rules.Remove(rule);
            return true;
        }
        return false;
    }

    public void AddRule(RuleDetailsViewModel rule)
    {
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
