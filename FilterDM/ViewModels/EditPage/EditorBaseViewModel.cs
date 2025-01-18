using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;
using System;



namespace FilterDM.ViewModels.EditPage;

public  partial class EditorBaseViewModel : ObservableRecipient
{
    public EditorBaseViewModel Content { get; set; }


    [RelayCommand]
    private void CloseMe()
    {
        Messenger.Send(new EditorClosedEvent(this));
    }

    [ObservableProperty]
    private string _title;
    public virtual bool IsPartOf(BlockDetailsViewModel vm)
    { return false; }

    public virtual ObservableRecipient GetSelectedContext()
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateTitle()
    {
        throw new NotImplementedException();
    }
}
