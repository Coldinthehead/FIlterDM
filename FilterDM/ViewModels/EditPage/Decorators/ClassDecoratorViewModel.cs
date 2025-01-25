using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterCore;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
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
            SelectedCount--;
        }
        else
        {
            SelectedCount++;
        }
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    public ClassDecoratorViewModel() 
    {
        IEnumerable<ItemClassDetails> details = App.Current!.Services!.GetRequiredService<ItemClassesService>().GetItemClasses();
        List<ClassItemViewModel> checkList
            = details
                .Select(x => new ClassItemViewModel(x))
                .ToList();
        SelectList = new(checkList);

        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    public override void Apply(RuleModel model)
    {
        ClassConditionModel condition = model.AddClassCondition();
        foreach (ClassItemViewModel item in SelectList)
        {
            if (item.IsSelected)
            {
                condition.Add(item.Name);
            }
        }
    }

    public void SetModel(ClassConditionModel model)
    {
        foreach (var item in _selectList)
        {
            if (model.HasClass(item.Name))
            {
                item.IsSelected = true;
                SelectedCount++;
            }
        }
    }

    public override ModifierEditorViewModel GetEditor() => new ClassEditorViewModel(Rule, this);
}
