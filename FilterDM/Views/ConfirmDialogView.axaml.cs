using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FilterDM.ViewModels;

namespace FilterDM;

public partial class ConfirmDialogView : Window
{
    public ConfirmDialogView()
    {
        InitializeComponent();

        DataContextChanged += (_, _) =>
        {
            if (DataContext is ConfirmDialogViewModel vm)
            {
                vm.CloseDialogAction = result => Close(result); // Закриваємо з результатом
            }
        };
    }
}