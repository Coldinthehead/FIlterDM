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

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class TypeViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isSelected;

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
        if (viewModel.IsSelected)
        {
            SelectedTypes.Add(viewModel);
        }
        else
        {
            SelectedTypes.Remove(viewModel);
        }
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
    public TypeDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {

        Dictionary<string, List<ItemTypeDetails>> types = App.Current!.Services!.GetRequiredService<ItemTypeService>().GetItemTypes();
        List<TypeListViewModel> typeModels = [];
        List<TypeViewModel> selected = [];
        foreach (string typename in  types.Keys)
        {
            TypeListViewModel vm = new();
            vm.Title = typename;
            List<TypeViewModel> typeItemList = [];
            foreach (ItemTypeDetails detail in types[typename])
            {
                TypeViewModel typeModel = new()
                {
                    Name = detail.Name,
                    Description = detail.Tip != string.Empty ? detail.Tip :  "No Tip :(",
                };
                typeItemList.Add(typeModel);
            }
            vm.Types = new(typeItemList);
            typeModels.Add(vm);
        }
        TypeList = new(typeModels);
        SelectedTypes = new(selected);

        CurrentTypeList = TypeList.First();
    }

    public void SetModel(TypeConditionModel model)
    {
        foreach (TypeListViewModel category in TypeList)
        {
            foreach (var item in category.Types)
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
