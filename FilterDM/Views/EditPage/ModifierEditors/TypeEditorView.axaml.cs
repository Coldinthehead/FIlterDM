using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FilterDM.Views.EditPage.ModifierEditors;

public partial class TypeEditorView : UserControl
{
    public TypeEditorView()
    {
        InitializeComponent();
        AddButton.Click += AddButton_Click;
    }

    private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Flyout.ShowAttachedFlyout(GridPanel);
    }
}