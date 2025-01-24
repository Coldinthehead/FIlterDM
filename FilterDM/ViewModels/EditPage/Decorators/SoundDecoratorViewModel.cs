using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
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

    [RelayCommand]
    private void Play()
    {
        _soundService.Play(SelectedSound, SoundVolume);
    }

    private readonly SoundService _soundService;

    public SoundDecoratorViewModel(SoundService soundService)
    {
        Sounds = new([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]);
        SelectedSound = Sounds[0];
        SoundVolume = 300;
        _soundService = soundService;
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

    public override ModifierEditorViewModel GetEditor() => new SoundEditorViewModel(Rule, this);
}
