using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class BeamDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private bool _useBeam;
    partial void OnUseBeamChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ObservableCollection<string> _beamColors;

    [ObservableProperty]
    private Color _selectedBeamRGB;

    private static Color ColorNameToColor(string name)
    {
        switch (name)
        {
            case "Red":
            return Colors.Red;
            case "Green":
            return Colors.Green;
            case "Blue":
            return Colors.Blue;
            case "Brown":
            return Colors.Brown;
            case "White":
            return Colors.White;
            case "Yellow":
            return Colors.Yellow;
            case "Cyan":
            return Colors.Cyan;
            case "Gray":
            return Colors.Gray;
            case "Orange":
            return Colors.Orange;
            case "Pink":
            return Colors.Pink;
            case "Purple":
            return Colors.Purple;
            default:
            return Colors.AliceBlue;
        }
    }

    [ObservableProperty]
    private string _selectedBeamColor;
    partial void OnSelectedBeamColorChanged(string? oldValue, string newValue)
    {
        if (!string.Equals(oldValue, newValue) && newValue != null)
        {
            SelectedBeamRGB = ColorNameToColor(newValue);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [ObservableProperty]
    private bool _isBeamPermanent;
    partial void OnIsBeamPermanentChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private static ObservableCollection<string> _staticBeamColors = new(["Red", "Green", "Blue", "Brown", "White", "Yellow", "Cyan", "Gray", "Orange", "Pink", "Purple"]);


    public BeamDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        BeamColors = _staticBeamColors;
        SelectedBeamColor = BeamColors[0];
        IsBeamPermanent = false;
       
    }

    internal void SetModel(BeamDetails beam)
    {
        if (_staticBeamColors.Contains(beam.Color))
        {
        SelectedBeamColor = beam.Color;
        }
        else
        {
            SelectedBeamColor = _staticBeamColors[0];
        }
        IsBeamPermanent = beam.IsPermanent;
    }

    public override void Apply(RuleModel model)
    {
        model.EnableBeam();
        model.Beam!.Color = SelectedBeamColor;
        model.Beam!.IsPermanent = IsBeamPermanent;
    }

    public override ModifierEditorViewModel GetEditor() => new BeamEditorViewModel(Rule, this);
}
