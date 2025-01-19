using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FilterDM.Tests.ViewModel.Tests;

public class EventListener<T, V> : ObservableRecipient, IRecipient<T> where T : ValueChangedMessage<V>
{
    public bool Received = false;
    public V Playload;

    public EventListener()
    {
        Messenger.Register(this);
    }

    public void Receive(T message)
    {
        Received = true;
        Playload = message.Value;
    }

}

