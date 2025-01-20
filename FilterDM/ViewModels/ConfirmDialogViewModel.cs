using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDM.ViewModels;

public partial class OkDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _message;

    private TaskCompletionSource<bool>? _tcs;
    public Action<bool>? CloseDialogAction { get; set; } // Дія для закриття діалогу

    public Task<bool> ShowDialogAsync()
    {
        _tcs = new TaskCompletionSource<bool>();
        return _tcs.Task;
    }

    [RelayCommand]
    private void Yes()
    {
        _tcs?.SetResult(true); // Завершуємо діалог із результатом "Так"
        CloseDialogAction?.Invoke(true); // Закриття діалогу
    }
}

public partial class ConfirmDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _message;


    private TaskCompletionSource<bool>? _tcs;

    public Action<bool>? CloseDialogAction { get; set; } // Дія для закриття діалогу

    // Метод для виклику діалогу
    public Task<bool> ShowDialogAsync()
    {
        _tcs = new TaskCompletionSource<bool>();
        return _tcs.Task;
    }

    [RelayCommand]
    private void Yes()
    {
        _tcs?.SetResult(true); // Завершуємо діалог із результатом "Так"
        CloseDialogAction?.Invoke(true); // Закриття діалогу
    }

    [RelayCommand]
    private void No()
    {
        _tcs?.SetResult(false); // Завершуємо діалог із результатом "Ні"
        CloseDialogAction?.Invoke(false); // Закриття діалогу
    }
}
