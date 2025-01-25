using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FilterDM.ViewModels;

namespace FilterDM;

public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
        OKButton.Click += OKButton_Click;
    }

    private void OKButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}