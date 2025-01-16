
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace FilterDM.ViewModels.EditPage.Helper;
public partial class ClassTypeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title;

    private Action<ClassTypeViewModel> _deleteAction;
    [RelayCommand]
    private void DeleteMe()
    {
        _deleteAction?.Invoke(this);
    }

    public ClassTypeViewModel(string title, Action<ClassTypeViewModel> deleteAction = null)
    {
        Title = title;
        _deleteAction = deleteAction;
    }
}
