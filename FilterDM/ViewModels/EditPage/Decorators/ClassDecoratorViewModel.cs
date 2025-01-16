using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterCore.PoeData;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class ClassItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private string _name;



    public ClassItemViewModel(ItemClassDetails details)
    {
        Name = details.Name;
    }
}

[ObservableRecipientAttribute]
public partial class ClassDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ClassItemViewModel> _selectList;

    [ObservableProperty]
    private int _selectedCount;

    [RelayCommand]
    private void ItemSelected(ClassItemViewModel item)
    {
        if (!item.IsSelected)
        {
            Condition.Remove(item.Name);
            SelectedCount--;
        }
        else
        {
            Condition.Add(item.Name);
            SelectedCount++;
        }
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    public ClassConditionModel Condition => _condition;
    private ClassConditionModel _condition;
    public ClassDecoratorViewModel(RuleDetailsViewModel rule
        , ClassConditionModel condition
        , Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        _condition = condition;

        IEnumerable<ItemClassDetails> details = App.Current!.Services!.GetRequiredService<ItemClassesService>().GetItemClasses();
        List<ClassItemViewModel> checkList
            = details
                .Select(x => new ClassItemViewModel(x))
                .ToList();
        SelectList = new();
        foreach (var item in checkList)
        {
            SelectList.Add(item);
            if (condition.HasClass(item.Name))
            {
                item.IsSelected = true;
                SelectedCount++;
                
            }
        }
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }
}
