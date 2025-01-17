using Avalonia.Styling;
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
using System.Net;

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

    protected override async void DeleteCallback()
    {
        if (SelectedTypes.Count > 1)
        {
            var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete Type Filter with {SelectedTypes.Count} selected names?");
            if (confirm)
            {
                DeleteAction.Invoke(this);
            }
        }
        else
        {
            DeleteAction.Invoke(this);
        }
    }

    public Action<TypeViewModel> TypeSelector { get; set; }

    public TypeDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
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

    public void InitalizeFromEmpty()
    {
        TypeList = new(BuildEmptyList());
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

    public void ReleaseScope()
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

        foreach (var item in SelectedTypes)
        {
            item.IsSelected = false;
            item.TakenInScope = false;
        }
        TypeList = new(BuildEmptyList());

        List<string> selectedName = SelectedTypes.Select(x => x.Name).ToList();
        List<TypeViewModel> next = [];
        foreach (var cat in TypeList)
        {
            foreach (TypeViewModel name in cat.Types)
            {
                if (selectedName.Contains(name.Name))
                {
                    name.IsSelected = true;
                    next.Add(name);
                }
            }
        }


        SelectedTypes = new(next);
        CurrentTypeList = TypeList.First();
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

        foreach (var categoty in TypeList)
        {
            foreach (TypeViewModel itemName in categoty.Types)
            {
                if (selectedItems.Contains(itemName.Name) && itemName.IsSelected == false && itemName.TakenInScope == false)
                {
                    nextSelected.Add(itemName);
                    itemName.IsSelected = true;
                    itemName.TakenInScope = true;
                }
            }
        }
        SelectedTypes = new(nextSelected);
        CurrentTypeList = TypeList.First();
    }

    public static List<TypeListViewModel> BuildEmptyList()
    {
        Dictionary<string, List<ItemTypeDetails>> IterCategories = App.Current!.Services!.GetRequiredService<ItemTypeService>().GetItemTypes();
        List<TypeListViewModel> catagoryList = [];
        foreach (string currentCategoryName in IterCategories.Keys)
        {
            TypeListViewModel listViewModel = new();
            
            listViewModel.Title = currentCategoryName;
            List<TypeViewModel> itemNamesList = [];
            foreach (ItemTypeDetails detail in IterCategories[currentCategoryName])
            {
                TypeViewModel typeModel = new()
                {
                    Name = detail.Name,
                    Description = detail.Tip != string.Empty ? detail.Tip : "No Tip :(",
                };
                itemNamesList.Add(typeModel);
            }
            listViewModel.Types = new(itemNamesList);
            catagoryList.Add(listViewModel);
        }
        return catagoryList;
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
        foreach (TypeListViewModel category in TypeList)
        {
            foreach (TypeViewModel item in category.Types)
            {
                if (model.HasType(item.Name))
                {
                    item.IsSelected = true;
                    SelectedTypes.Add(item);
                }
            }
        }
    }
}
