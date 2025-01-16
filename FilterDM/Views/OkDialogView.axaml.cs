using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FilterDM.ViewModels;

namespace FilterDM;

public partial class OkDialogView : Window
{
    public OkDialogView()
    {
        InitializeComponent();

        DataContextChanged += (_, _) =>
        {
            if (DataContext is OkDialogViewModel vm)
            {
                vm.CloseDialogAction = result => Close(result); // Закриваємо з результатом
            }
        };
    }
}