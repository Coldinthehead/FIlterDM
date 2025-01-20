using Avalonia.Controls.Primitives;
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
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;

public partial class AddModifierViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _title;

    [ObservableProperty]
    public string _isApplied;

    [RelayCommand]
    public void AddMe()
    {
        OnAddAction?.Invoke();
    }

    public Action OnAddAction { get; set; }

    public static AddModifierViewModel Build(string title, Action addAction)
    {
        return new AddModifierViewModel()
        {
            Title = title,
            OnAddAction = addAction,
        };
    }
}

public partial class RuleEditorViewModel : EditorBaseViewModel
    , IRecipient<RuleModifierDeleteEvent>
    , IRecipient<RuleTitleApplyEvent>
{

    [ObservableProperty]
    private RuleDetailsViewModel _rule;


    [ObservableProperty]
    private ObservableCollection<AddModifierViewModel> _addModifiersList;


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

        List<AddModifierViewModel> modifers = [
            AddModifierViewModel.Build("Text Size", AddFontSizeModifier),
            AddModifierViewModel.Build("Colors", AddColorsModifier),
            AddModifierViewModel.Build("Beam", AddFontSizeModifier),
            AddModifierViewModel.Build("Icon", AddMinimapIconModifier),
            AddModifierViewModel.Build("Sound", AddSoundModifier),
            AddModifierViewModel.Build("Class", AddClassFilter),
            AddModifierViewModel.Build("Type", AddTypeFilter),
            AddModifierViewModel.Build("Rarity", AddRarityFilter),
            AddModifierViewModel.Build("Item Level", ()=> AddNumericFilter(NumericFilterType.ItemLevel)),
            AddModifierViewModel.Build("Drop Level", ()=> AddNumericFilter(NumericFilterType.DropLevel)),
            AddModifierViewModel.Build("Area Level", ()=> AddNumericFilter(NumericFilterType.AreaLevel)),
            AddModifierViewModel.Build("Quality", ()=> AddNumericFilter(NumericFilterType.Quality)),
            AddModifierViewModel.Build("Sockets", ()=> AddNumericFilter(NumericFilterType.Sockets)),
            AddModifierViewModel.Build("Base Armour", ()=> AddNumericFilter(NumericFilterType.BaseArmour)),
            AddModifierViewModel.Build("Base Evasion", ()=> AddNumericFilter(NumericFilterType.BaseEvasion)),
            AddModifierViewModel.Build("Base ES", ()=> AddNumericFilter(NumericFilterType.BaseEnergyShield)),
            AddModifierViewModel.Build("Waystone Tier", ()=> AddNumericFilter(NumericFilterType.BaseEnergyShield))
      ];
        AddModifiersList = new(modifers);
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
