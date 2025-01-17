using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class SoundDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private bool _useSound;
    partial void OnUseSoundChanged(bool oldValue, bool newValue)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ObservableCollection<int> _sounds;

    [ObservableProperty]
    private int _selectedSound;
    partial void OnSelectedSoundChanged(int value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private int _soundVolume;

    partial void OnSoundVolumeChanged(int value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    public SoundDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        Sounds = new([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]);
        SelectedSound = Sounds[0];
        SoundVolume = 300;
    }

    public override void Apply(RuleModel model)
    {
        model.EnableSound();
        model.Sound!.Sample = SelectedSound;
        model.Sound.Volume = SoundVolume;
    }

    internal void SetModel(SoundDetails sound)
    {
        SelectedSound = sound.Sample;
        SoundVolume = sound.Volume;
    }

}
