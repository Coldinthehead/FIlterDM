using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterCore.PoeData;
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
using System.Net;
using System.Reflection;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class TypeViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _takenInScope = false;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _description;
}

public partial class TypeListViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private ObservableCollection<TypeViewModel> _types;

    public TypeListViewModel()
    {

    }
}
     

public partial class TypeDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<TypeViewModel> _selectedTypes;


    [ObservableProperty]
    private ObservableCollection<TypeListViewModel> _typeList;

    [ObservableProperty]
    private TypeListViewModel _currentTypeList;
    partial void OnCurrentTypeListChanged(TypeListViewModel? oldValue, TypeListViewModel newValue)
    {
        if (newValue != null && !newValue.Equals(oldValue))
        {
            CurrentTypeNames = newValue.Types;
        }
    }

    [ObservableProperty]
    private ObservableCollection<TypeViewModel> _currentTypeNames;


    [RelayCommand]
    private void TypeSelected(TypeViewModel viewModel)
    {
        TypeSelector.Invoke(viewModel);
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    [RelayCommand]
    private void Clear()
    {
        foreach (var item in SelectedTypes)
        {
            item.IsSelected = false;
        }
        SelectedTypes.Clear();
    }

    public Action<TypeViewModel> TypeSelector { get; set; }

    public TypeDecoratorViewModel()
    {
        SelectedTypes = new();
        TypeSelector = (vm) =>
        {
            if (vm.IsSelected)
            {
                SelectedTypes.Add(vm);
            }
            else
            {
                SelectedTypes.Remove(vm);
            }
        };
    }

    public void InitalizeFromList(List<TypeListViewModel> categories)
    {
        TypeList = new(categories);
        SelectedTypes = new();
        CurrentTypeList = TypeList.First();
    }

    public void InitalizeFromEmpty(List<TypeListViewModel> categories)
    {
        TypeList = new(categories);
        CurrentTypeList = TypeList.First();
        TypeSelector = (vm) =>
        {
            if (vm.IsSelected)
            {
                SelectedTypes.Add(vm);
            }
            else
            {
                SelectedTypes.Remove(vm);
            }
        };
    }

    public void ReleaseScope(List<TypeListViewModel> categories)
    {
        TypeSelector = (vm) =>
        {
            if (vm.IsSelected)
            {
                SelectedTypes.Add(vm);
            }
            else
            {
                SelectedTypes.Remove(vm);
            }
        };
        //cache selected names before clear;
        List<string> selectedName = SelectedTypes.Select(x => x.Name).ToList();

        foreach (var item in SelectedTypes)
        {
            item.IsSelected = false;
            item.TakenInScope = false;
        }
        TypeList = new(categories);

       
        List<TypeViewModel> next = [];
        Dictionary<TypeListViewModel, int> _counts = [];
        foreach (TypeListViewModel category in TypeList)
        {
            _counts[category] = 0;
            foreach (TypeViewModel name in category.Types)
            {
                if (selectedName.Contains(name.Name))
                {
                    name.IsSelected = true;
                    next.Add(name);
                    _counts[category]++;
                }
            }
        }


        SelectedTypes = new(next);
        CurrentTypeList = _counts.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
    }

    internal void SetScoped(List<TypeListViewModel> scopel)
    {
        TypeList = new(scopel);
        TypeSelector = (vm) =>
        {
            if (vm.IsSelected)
            {
                SelectedTypes.Add(vm);
                vm.TakenInScope = true;
            }
            else
            {
                SelectedTypes.Remove(vm);
                vm.TakenInScope = false;
            }
        };

        List<TypeViewModel> nextSelected = [];
        var selectedItems = SelectedTypes.Select(t => t.Name).ToList();


        Dictionary<TypeListViewModel, int> _counts = [];
        foreach (var category in TypeList)
        {
            _counts[category] = 0;
            foreach (TypeViewModel itemName in category.Types)
            {
                if (selectedItems.Contains(itemName.Name) && itemName.IsSelected == false && itemName.TakenInScope == false)
                {
                    nextSelected.Add(itemName);
                    itemName.IsSelected = true;
                    itemName.TakenInScope = true;
                    _counts[category]++;
                }
            }
        }
        SelectedTypes = new(nextSelected);
        CurrentTypeList = _counts.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
    }

   

    public override void Apply(RuleModel model)
    {
        TypeConditionModel condition = model.AddTypeCondition();
        foreach (TypeViewModel selectedItem in SelectedTypes)
        {
            condition.SelectedTypes.Add(selectedItem.Name);
        }
    }

    public void SetModel(TypeConditionModel model)
    {
        Dictionary<TypeListViewModel, int> _counts = [];
        foreach (TypeListViewModel category in TypeList)
        {
            _counts[category] = 0;
            foreach (TypeViewModel item in category.Types)
            {
                if (model.HasType(item.Name))
                {
                    item.IsSelected = true;
                    SelectedTypes.Add(item);
                    _counts[category]++;
                }
            }
        }
        CurrentTypeList = _counts.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
    }

    public void SetModelScoped(TypeConditionModel model)
    {
        Dictionary<TypeListViewModel, int> _counts = [];
        foreach (TypeListViewModel category in TypeList)
        {
            _counts[category] = 0;
            foreach (TypeViewModel item in category.Types)
            {
               
                if (model.HasType(item.Name) && item.TakenInScope == false)
                {
                    item.IsSelected = true;
                    item.TakenInScope = true;
                    SelectedTypes.Add(item);
                    _counts[category]++;
                }
            }
        }
        CurrentTypeList = _counts.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
    }

    public override ModifierEditorViewModel GetEditor() => new TypeEditorViewModel(Rule, this);
  
}
