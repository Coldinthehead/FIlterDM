using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;



namespace FilterDM.ViewModels.EditPage;

public  partial class EditorBaseViewModel : ObservableRecipient
{
    public EditorBaseViewModel Content { get; set; }


    [RelayCommand]
    private void CloseMe()
    {
    }

    [ObservableProperty]
    private string _title;
    public virtual bool IsPartOf(BlockDetailsViewModel vm)
    { return false; }
}
