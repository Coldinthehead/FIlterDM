﻿using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Enums;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;
using System.Collections.Generic;

namespace FilterDM.ViewModels.EditPage;
public partial class RuleEditorViewModel : EditorBaseViewModel
    , IRecipient<RuleModifierDeleteEvent>
    , IRecipient<RuleTitleApplyEvent>
{

    [ObservableProperty]
    private RuleDetailsViewModel _rule;

    [ObservableProperty]
    private ModifierViewModelBase _selectedModifier;
    partial void OnSelectedModifierChanged(ModifierViewModelBase? oldValue, ModifierViewModelBase newValue)
    {
        if (newValue != null && !newValue.Equals(oldValue))
        {
            CurrentModifierEditor = GetEditorModelByType(newValue);
        }
    }

    [ObservableProperty]
    private ViewModelBase _currentModifierEditor;


    [RelayCommand]
    private void AddModifier(string modeName)
    {
        switch (modeName)
        {
            case "FontSize":
            AddFontSizeModifier();
            break;

            case "Colors":
            AddColorsModifier();
            break;
            case "Beam":
            AddBeamModifier();
            break;
            case "Icon":
            AddMinimapIconModifier();
            break;

            case "Sound":
            AddSoundModifier();
            break;

            case "ClassFilter":
            AddClassFilter();
            break;
            case "TypeFilter":
            AddTypeFilter();
            break;
            case "RarityFilter":
            AddRarityFilter();
            break;
            case "StackFilter":
            AddNumericFilter(NumericFilterType.StackSize);
            break;
            case "ItemLevelFilter":
            AddNumericFilter(NumericFilterType.ItemLevel);
            break;
            case "DropLevelFilter":
            AddNumericFilter(NumericFilterType.DropLevel);
            break;
            case "AreaLevelFilter":
            AddNumericFilter(NumericFilterType.AreaLevel);
            break;
            case "QualityFilter":
            AddNumericFilter(NumericFilterType.Quality);
            break;
            case "SocketFilter":
            AddNumericFilter(NumericFilterType.Sockets);
            break;
            case "ArmorFilter":
            AddNumericFilter(NumericFilterType.BaseArmour);
            break; 
            case "EvasionFilter":
            AddNumericFilter(NumericFilterType.BaseEvasion);
            break;  
            case "ESFilter":
            AddNumericFilter(NumericFilterType.BaseEnergyShield);
            break;

            default:
            break;
        }
    }

    private static Dictionary<Type, Type> _editorTypesToModifierTypes = [];

    static RuleEditorViewModel()
    {
        _editorTypesToModifierTypes.Add(typeof(FontSizeEditorViewModel), typeof(TextSizeDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(ColorEditorViewModel), typeof(ColorDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(BeamEditorViewModel), typeof(BeamDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(MinimapIconEditorViewModel), typeof(MapIconDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(SoundEditorViewModel), typeof(SoundDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(RarityEditorViewModel), typeof(RarityDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(NumericEditorViewModel), typeof(NumericDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(ClassEditorViewModel), typeof(ClassDecoratorViewModel));
        _editorTypesToModifierTypes.Add(typeof(TypeEditorViewModel), typeof(TypeDecoratorViewModel));
    }

    public RuleEditorViewModel(RuleDetailsViewModel rule)
    {
        Rule = rule;
        Content = this;
        Title = rule.Properties.Title;
        SelectedModifier = Rule.Modifiers[0];
        Messenger.Register<RuleModifierDeleteEvent>(this);
        Messenger.Register<RuleTitleApplyEvent, RuleDetailsViewModel>(this, Rule);
        
    }

    private void AddFontSizeModifier()
    {
        SelectedModifier = Rule.AddFontSizeModifier();
    }

    private void AddColorsModifier()
    {
        SelectedModifier = Rule.AddColorsModifier();
    }

    private void AddBeamModifier()
    {
        SelectedModifier = Rule.AddBeamModifier();
    }

    private void AddMinimapIconModifier()
    {
        SelectedModifier = Rule.AddMinimapIconModifier();
    }

    private void AddSoundModifier()
    {
        SelectedModifier = Rule.AddSoundModifier();
    }

    private void AddRarityFilter()
    {
        SelectedModifier = Rule.AddRarityFilter();
    }

    private void AddNumericFilter(NumericFilterType type)
    {
        SelectedModifier = Rule.AddNumericFilter(type);
    }

    private void AddClassFilter()
    {
        SelectedModifier = Rule.AddClassFilter();
    }

    private void AddTypeFilter()
    {
        SelectedModifier = Rule.AddTypeFilter();
    }

    public ViewModelBase GetEditorModelByType(ModifierViewModelBase vm)
    {
        Type t = vm.GetType();
        if (vm is TextSizeDecoratorViewModel text)
        {
            return new FontSizeEditorViewModel(Rule, text);
        }
        else if (vm is ColorDecoratorViewModel color)
        {
            return new ColorEditorViewModel(Rule, color);
        }
        else if (vm is BeamDecoratorViewModel beam)
        {
            return new BeamEditorViewModel(Rule, beam);
        }
        else if (vm is MapIconDecoratorViewModel minimapIcon)
        {
            return new MinimapIconEditorViewModel(Rule, minimapIcon);
        }
        else if (vm is SoundDecoratorViewModel sound)
        {
            return new SoundEditorViewModel(Rule, sound);
        }
        else if (vm is RarityDecoratorViewModel rarity)
        {
            return new RarityEditorViewModel(Rule, rarity);
        }
        else if (vm is NumericDecoratorViewModel numeric)
        {
            return new NumericEditorViewModel(Rule, numeric);
        }
        else if (vm is RulePropertiesDecoratorViewModel properties)
        {
            return new RulePropertiesEditorViewModel(Rule, properties);
        }
        else if (vm is ClassDecoratorViewModel classDec)
        {
            return new ClassEditorViewModel(Rule, classDec);
        }
        else if (vm is TypeDecoratorViewModel typeDec)
        {
            return new TypeEditorViewModel(Rule, typeDec);
        }
        else
        {
            return null;
        }
    }

    public override bool IsPartOf(BlockDetailsViewModel vm)
    {
        return vm.Rules.Contains(Rule);
    }

    public void Receive(RuleModifierDeleteEvent message)
    {
        if (CurrentModifierEditor != null && MatchEditorType(CurrentModifierEditor.GetType(), message.Value.GetType()))
        {
            CurrentModifierEditor = null;
        }
    }

    private bool MatchEditorType(Type editor, Type modifier)
    {
        if (!_editorTypesToModifierTypes.ContainsKey(editor))
        {
            return false;
        }
        return _editorTypesToModifierTypes[editor] == modifier;
    }

    public override void UpdateTitle()
    {
        Title = Rule.Properties.Title;
    }

    public override ObservableRecipient GetSelectedContext()
    {
        return Rule;
    }

    public override bool Equals(object? obj)
    {
        if (obj is RuleEditorViewModel other)
        {
            return other.Rule == this.Rule;
        }
        return false;
    }

    public void Receive(RuleTitleApplyEvent message)
    {
        this.Title = message.Value.Properties.Title;
    }
}
