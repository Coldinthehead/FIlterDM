using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;


namespace FilterDM.ViewModels.EditPage;

public  partial class EditorBaseViewModel : ObservableRecipient
{
    public EditorBaseViewModel Content { get; set; }

    public Action<EditorBaseViewModel> CloseAction { get; set; }

    [RelayCommand]
    private void CloseMe()
    {
        CloseAction?.Invoke(this);
    }

    [ObservableProperty]
    private string _title;
    public virtual bool IsPartOf(BlockDetailsViewModel vm)
    { return false; }
}
