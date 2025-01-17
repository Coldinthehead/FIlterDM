using Avalonia.Controls;
using FilterDM.ViewModels;
using System;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class DialogService
{
    private readonly Window _target;

    public DialogService(Window target)
    {
        _target = target;
    }

    public async Task<bool> ShowConfirmDialog(string message)
    {
        var viewModel = new ConfirmDialogViewModel()
        {
            Message = message,
        };

        // Ініціалізація діалогу
        var dialog = new ConfirmDialogView
        {
            DataContext = viewModel // Прив'язуємо ViewModel до вікна
        };

        // Показуємо діалогове вікно і чекаємо результат
        var result = await dialog.ShowDialog<bool>(_target);
        return result;
    }

    public async Task<bool> ShowOkDialog(string message)
    {
        var viewModel = new OkDialogViewModel()
        {
            Message = message,
        };

        // Ініціалізація діалогу
        var dialog = new OkDialogView
        {
            DataContext = viewModel // Прив'язуємо ViewModel до вікна
        };

        // Показуємо діалогове вікно і чекаємо результат
        var result = await dialog.ShowDialog<bool>(_target);
        return result;
    }

    internal async Task<bool> ShowError(string message)
    {
        var viewModel = new OkDialogViewModel()
        {
            Message = message,
        };

        // Ініціалізація діалогу
        var dialog = new ErrorWindow
        {
            DataContext = viewModel // Прив'язуємо ViewModel до вікна
        };

        // Показуємо діалогове вікно і чекаємо результат
        var result = await dialog.ShowDialog<bool>(_target);
        return result;

    }
}
