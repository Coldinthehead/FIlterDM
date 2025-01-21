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
using System.Reflection;

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
            CurrentModifierEditor = newValue.GetEditor();
        }
    }

    [ObservableProperty]
    private ModifierEditorViewModel _currentModifierEditor;


    public RuleEditorViewModel(RuleDetailsViewModel rule)
    {
        Rule = rule;
        Content = this;
        Title = rule.Properties.Title;
        SelectedModifier = Rule.Modifiers[0];

        List<AddModifierViewModel> modifers = [
            AddModifierViewModel.Build("Text Size", AddFontSizeModifier),
            AddModifierViewModel.Build("Colors", AddColorsModifier),
            AddModifierViewModel.Build("Beam", AddBeamModifier),
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
            AddModifierViewModel.Build("Waystone Tier", ()=> AddNumericFilter(NumericFilterType.WaystoneTier))
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

    public override bool IsPartOf(BlockDetailsViewModel vm)
    {
        return vm.Rules.Contains(Rule);
    }

    public void Receive(RuleModifierDeleteEvent message)
    {
        if (CurrentModifierEditor != null && CurrentModifierEditor.Rule == message.Value.Rule)
        {
            CurrentModifierEditor = null;
        }
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
