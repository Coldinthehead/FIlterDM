using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Enums;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage;

public partial class AddNumericModifier : AddModifierViewModel
{
    public NumericFilterType FilterType { get; protected set; }

    public static AddNumericModifier Build(string title, int maxCount, NumericFilterType filterType, Action addAction)
    {
        return new AddNumericModifier()
        {
            Title = title,
            OnAddAction = addAction,
            _maxCount = maxCount,
            ModifierType = typeof(NumericDecoratorViewModel),
            FilterType = filterType
        };
    }
}

public partial class AddModifierViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _title;

    [ObservableProperty]
    public bool _canApply = true;

    [RelayCommand]
    public void AddMe()
    {
        IncrementCount();
        OnAddAction?.Invoke();
    }

    protected int _maxCount;
    protected int _count = 0;
    public Type ModifierType { get; protected set; }

    public Action OnAddAction { get; set; }

    public void IncrementCount()
    {
        _count++;
        if (_count == _maxCount)
        {
            CanApply = false;
        }
    }

    public void DecrementCount()
    {
        _count--;
        if (_count == 0)
        {
            CanApply = true;
        }
    }
    public static AddModifierViewModel Build<T>(string title, Action addAction) where T : ModifierViewModelBase
    {
        return Build<T>(title, 1, addAction);
    }

    public static AddModifierViewModel Build<T>(string title, int maxCount, Action addAction) where T : ModifierViewModelBase
    {
        return new AddModifierViewModel()
        {
            Title = title,
            OnAddAction = addAction,
            _maxCount = maxCount,
            ModifierType = typeof(T),
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
            AddModifierViewModel.Build<TextSizeDecoratorViewModel>("Text Size", AddFontSizeModifier),
            AddModifierViewModel.Build<ColorDecoratorViewModel>("Colors", AddColorsModifier),
            AddModifierViewModel.Build<BeamDecoratorViewModel>("Beam", AddBeamModifier),
            AddModifierViewModel.Build<MapIconDecoratorViewModel>("Icon", AddMinimapIconModifier),
            AddModifierViewModel.Build<SoundDecoratorViewModel>("Sound", AddSoundModifier),
            AddModifierViewModel.Build<ClassDecoratorViewModel>("Class", AddClassFilter),
            AddModifierViewModel.Build<TypeDecoratorViewModel>("Type", AddTypeFilter),
            AddModifierViewModel.Build<RarityDecoratorViewModel>("Rarity", AddRarityFilter),
            AddNumericModifier.Build("Item Level",2,NumericFilterType.ItemLevel, ()=> AddNumericFilter(NumericFilterType.ItemLevel)),
            AddNumericModifier.Build("Drop Level", 2, NumericFilterType.DropLevel,()=> AddNumericFilter(NumericFilterType.DropLevel)),
            AddNumericModifier.Build("Area Level", 2, NumericFilterType.AreaLevel,() => AddNumericFilter(NumericFilterType.AreaLevel)),
            AddNumericModifier.Build("Quality", 2, NumericFilterType.Quality,() => AddNumericFilter(NumericFilterType.Quality)),
            AddNumericModifier.Build("Sockets", 2, NumericFilterType.Sockets,() => AddNumericFilter(NumericFilterType.Sockets)),
            AddNumericModifier.Build("Base Armour", 2, NumericFilterType.BaseArmour,() => AddNumericFilter(NumericFilterType.BaseArmour)),
            AddNumericModifier.Build("Base Evasion", 2, NumericFilterType.BaseEvasion,() => AddNumericFilter(NumericFilterType.BaseEvasion)),
            AddNumericModifier.Build("Base ES", 2, NumericFilterType.BaseEnergyShield,() => AddNumericFilter(NumericFilterType.BaseEnergyShield)),
            AddNumericModifier.Build("Waystone Tier", 2, NumericFilterType.WaystoneTier,() => AddNumericFilter(NumericFilterType.WaystoneTier))
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
        List<AddModifierViewModel> buttons = AddModifiersList.Where(x => x.ModifierType == message.Value.GetType()).ToList();
        if (buttons.Count == 1)
        {
            buttons.First().DecrementCount();
        }
        else
        {
            AddModifierViewModel btn = buttons.Select(x => (AddNumericModifier)x).Where(x => x != null && x.FilterType == ((NumericDecoratorViewModel)message.Value).FilterType).First();
            if (btn != null)
            {
                btn.DecrementCount();
            }
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
