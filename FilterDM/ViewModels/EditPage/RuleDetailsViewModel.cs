using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace FilterDM.ViewModels.EditPage;

public partial class RuleDetailsViewModel : ObservableRecipient , IEquatable<RuleDetailsViewModel>
{
    [ObservableProperty]
    private ObservableCollection<ViewModelBase> _modifiers;

    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _allBlocks;

    [ObservableProperty]
    private BlockDetailsViewModel _selectedParent;

    [ObservableProperty]
    private string _selectedTemplate;
    partial void OnSelectedTemplateChanged(string? oldValue, string newValue)
    {
        if (newValue != null && newValue != string.Empty)
        {
            _model.TemplateName = newValue;
        }
    }

    public BlockDetailsViewModel RealParent => _realParent;
    private BlockDetailsViewModel _realParent;

    [RelayCommand]
    private async void DeleteMe()
    {
        if (Modifiers.Count > 1)
        {
            var dialogResult = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete Rule with {Modifiers.Count} modifiers?");
            if (dialogResult)
            {
                Messenger.Send(new RuleDeleteRequestEvent(this));
            }
        }
        else
        {
            Messenger.Send(new RuleDeleteRequestEvent(this));
        }
       
    
    }

    [RelayCommand]
    private void ApplyProperties()
    {
        if (SelectedParent == null || _realParent == null)
        {
            return;
        }

        if (RealParent == SelectedParent)
        {
            SelectedParent.SortRules();
        }
        else
        {
            RealParent.DeleteRule(this);
            _realParent = SelectedParent;
            SelectedParent.AddRule(this);
        }

        Messenger.Send(new RuleTitleApplyEvent(this), this);
    }

    [RelayCommand]
    private void Reset()
    {
        if (SelectedTemplate != null)
        {
            RuleTemplateService? service = App.Current.Services.GetService<RuleTemplateService>();
            RuleModel? nextTempate = service.GetTemplate(SelectedTemplate);
            if (nextTempate != null)
            {
                nextTempate.Title = _model.Title;
                RealParent.Model.DeleteRule(_model);
                SetModel(nextTempate);
                RealParent.Model.AddRule(nextTempate);
            }
        }
    }

    #region Colors

    [ObservableProperty]
    private bool _useAnyColor;

    [ObservableProperty]
    private bool _useFontColor = false;
    partial void OnUseFontColorChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            TextColor = _cachedFontColor;
            UseAnyColor = true;
        }
        else
        {
            _cachedFontColor = TextColor;
            TextColor = DEFAULT_FONT_COLOR;
            _model.RemoveTextColor();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private Color _textColor = DEFAULT_FONT_COLOR;
    partial void OnTextColorChanged(Color value)
    {
        if (UseFontColor)
        {
            _model.AddTextColor(value);
          
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private Color _cachedFontColor = DEFAULT_FONT_COLOR;
    private static Color DEFAULT_FONT_COLOR = new Color(255, 100, 97, 87);


    [ObservableProperty]
    private bool _useBorderColor = false;
    partial void OnUseBorderColorChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            UseAnyColor = true;
            BorderColor = _cachedBorderColor;
        }
        else
        {
            _cachedBorderColor = BorderColor;
            BorderColor = DEFAULT_BORDER_COLOR;
            _model.RemoveBroderColor();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private Color _borderColor = DEFAULT_BORDER_COLOR;
    partial void OnBorderColorChanged(Color value)
    {
        if (UseBorderColor)
        {
            _model.AddBorderColor(value);
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    private Color _cachedBorderColor = DEFAULT_BORDER_COLOR;

    private static Color DEFAULT_BORDER_COLOR = Colors.Black;


    [ObservableProperty]
    private bool _useBackColor = false;
    partial void OnUseBackColorChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            UseAnyColor = true;
            BackColor = _cachedBackColor;
        }
        else
        {
            _cachedBackColor = BackColor;
            BackColor = DEFAULT_BACK_COLOR;
            _model.RemoveBackgroundColor();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private Color _backColor = DEFAULT_BACK_COLOR;
    partial void OnBackColorChanged(Color value)
    {
        if (UseBackColor)
        {
            _model.AddBackgroundColor(value);
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    private Color _cachedBackColor = DEFAULT_BACK_COLOR;

    private static Color DEFAULT_BACK_COLOR = Colors.Black;

    #endregion

    #region InGameDecorators

    [ObservableProperty]
    private bool _useBeam;
    partial void OnUseBeamChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            _model.EnableBeam();
        }
        else
        {
            _model.DisableBeam();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private bool _useMinimapIcon;
    partial void OnUseMinimapIconChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            _model.EnableIcon();
        }
        else
        {
            _model.DisableIcon();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private bool _useSound;
    partial void OnUseSoundChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            _model.EnableSound();
        }
        else
        {
            _model.DisableSound();
        }
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
            _model.SetBeamColor(newValue);
            SelectedBeamRGB = ColorNameToColor(newValue);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    [ObservableProperty]
    private bool _isBeamPermanent;
    partial void OnIsBeamPermanentChanged(bool value)
    {
        _model.SetBeamLifetime(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ObservableCollection<string> _iconSizes;

    [ObservableProperty]
    private string _selectedIconSize;
    partial void OnSelectedIconSizeChanged(string value)
    {
        _model.SetIconSize(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private string _selectedIconColor;
    partial void OnSelectedIconColorChanged(string value)
    {
        _model.SetIconColor(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ObservableCollection<string> _iconShapes;

    [ObservableProperty]
    private string _selectedShape;
    partial void OnSelectedShapeChanged(string value)
    {
        _model.SetIconShape(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ObservableCollection<int> _sounds;

    [ObservableProperty]
    private int _selectedSound;
    partial void OnSelectedSoundChanged(int value)
    {
        _model.SetSoundSample(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private int _soundVolume;

    partial void OnSoundVolumeChanged(int value)
    {
        _model.SetSoundVolume(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    #endregion

    #region RuleProperties


    [ObservableProperty]
    private string _title;
    partial void OnTitleChanged(string value)
    {
        _model.SetTittle(value);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private bool _enabled;
    partial void OnEnabledChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            _model.Enabled = newValue;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private float _priority;
    partial void OnPriorityChanged(float oldValue, float newValue)
    {
        if (oldValue != newValue)
        {
            _model.Priority = newValue;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private bool _show;
    partial void OnShowChanged(bool value)
    {
        _model.Show = value;
    }

    #endregion

    #region Misc Decrotators

    [ObservableProperty]
    private bool _useFontSize = false;
    partial void OnUseFontSizeChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            FontSize = _cachedFontSize;
            _model.FontSize = (int)FontSize;
        }
        else
        {
            _cachedFontSize = FontSize;
            FontSize = DEFAULT_FONT_SIZE;
            _model.FontSize = (int)FontSize;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private static float DEFAULT_FONT_SIZE = 32;
    [ObservableProperty]
    private float _fontSize = DEFAULT_FONT_SIZE;
    partial void OnFontSizeChanged(float oldValue, float newValue)
    {
        _model.FontSize = (int)newValue;
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    private float _cachedFontSize = DEFAULT_FONT_SIZE;
    #endregion

    #region Filters

    [ObservableProperty]
    private bool _useRarityFilter;

    [ObservableProperty]
    private bool _useStackFilter;

    [ObservableProperty]
    private bool _useItemLevelFilter;

    [ObservableProperty]
    private bool _useDropLevelFilter;

    [ObservableProperty]
    private bool _useAreaLevelFilter;

    [ObservableProperty]
    private bool _useQualityFilter;

    [ObservableProperty]
    private bool _useSocketFilter;

    [ObservableProperty]
    private bool _useArmorFilter;
    [ObservableProperty]
    private bool _useEvasionFilter;
    [ObservableProperty]
    private bool _useESFilter;

    [ObservableProperty]
    private bool _useClassFilter;

    [ObservableProperty]
    private bool _useNameFilter;

    [ObservableProperty]
    private bool _useWaystoneFilter;

    #endregion

    #region Moidifiers Methods

    public TextSizeDecoratorViewModel AddFontSizeModifier()
    {
        UseFontSize = true;
        TextSizeDecoratorViewModel vm = new(this, RemoveFontSizeModifierFromList);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public ViewModelBase AddColorsModifier()
    {
        ColorDecoratorViewModel vm = new(this, RemoveColorModifier);
        UseAnyColor = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public ViewModelBase AddBeamModifier()
    {
        BeamDecoratorViewModel vm = new(this, RemoveBeamModifier);
        UseBeam = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;

    }

    public ViewModelBase AddMinimapIconModifier()
    {
        MapIconDecoratorViewModel vm = new(this, RemoveMinimapIconModifier);
        UseMinimapIcon = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public ViewModelBase AddSoundModifier()
    {
        SoundDecoratorViewModel vm = new(this, RemoveSoundModifier);
        UseSound = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public ViewModelBase AddRarityFilter()
    {
        RarityDecoratorViewModel vm = new(this, _model.AddRarityCondition(), RemoveRarityFilter);
        UseRarityFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddRarityFilter(RarityConditionModel model)
    {
        RarityDecoratorViewModel vm = new RarityDecoratorViewModel(this, model, RemoveRarityFilter);
        UseRarityFilter = true;
        Messenger.Send(new FilterEditedRequestEvent(this));
        Modifiers.Add(vm);
    }

    private Dictionary<NumericFilterType, Action<bool>> _numericActions = [];
    public ViewModelBase AddNumericFilter(NumericFilterType type)
    {
        NumericCondition condition = _model.AddNumericCondition();
        NumericFilterHelper helper = _numericHelpers[type];
        condition.ValueName = helper.Name;
        NumericDecoratorViewModel vm = new(this, condition, type, helper, RemoveNumericFilter);
        Modifiers.Add(vm);
        helper.Add();
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    private void AddNumericFilter(NumericCondition condition)
    {
        string name = condition.ValueName.Replace(" ", "");

        if (_helperFromString.ContainsKey(name))
        {
            NumericFilterHelper helper = _helperFromString[name];
            NumericDecoratorViewModel vm = new(this, condition, helper.Type, helper, RemoveNumericFilter);
            Modifiers.Add(vm);
            helper.Add();
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public ViewModelBase AddClassFilter()
    {
        ClassConditionModel condition = _model.AddClassCondition();
        ClassDecoratorViewModel vm = new(this, condition, RemoveClassFilter);
        Messenger.Send(new FilterEditedRequestEvent(this));
        UseClassFilter = true;
        Modifiers.Add(vm);
        return vm;
    }

    public void AddClassFilter(ClassConditionModel condition)
    {
        ClassDecoratorViewModel vm = new(this, condition, RemoveClassFilter);
        UseClassFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public ViewModelBase AddTypeFilter()
    {
        TypeConditionModel condition = _model.AddTypeCondition();
        TypeDecoratorViewModel vm = new TypeDecoratorViewModel(this, condition, RemoveTypeFilter);
        UseNameFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddTypeFilter(TypeConditionModel condition)
    {
        TypeDecoratorViewModel vm = new TypeDecoratorViewModel(this, condition, RemoveTypeFilter);
        UseNameFilter = true;
        Messenger.Send(new FilterEditedRequestEvent(this));
        Modifiers.Add(vm);
    }

    private void RemoveTypeFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is TypeDecoratorViewModel condition)
        {
            UseNameFilter = false;
            Model.RemoveTypeCondition();
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveClassFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is ClassDecoratorViewModel condition)
        {
            UseClassFilter = false;
            Model.RemoveClassTypeCondition();
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveNumericFilter(ModifierViewModelBase modifier)
    {
        if (modifier is NumericDecoratorViewModel condition && Modifiers.Remove(modifier))
        {
            _numericHelpers[condition.FilterType].Remove();
            Model.RemoveNumericCondition(condition.Model);
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveRarityFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseRarityFilter = false;
            Model.RemoveRarityCondition();
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveSoundModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseSound = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveMinimapIconModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseMinimapIcon = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveColorModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseAnyColor = false;
            UseFontColor = false;
            UseBorderColor = false;
            UseBackColor = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }

    }

    private void RemoveBeamModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseBeam = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveFontSizeModifierFromList(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseFontSize = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public bool Equals(RuleDetailsViewModel? other)
    {
        return _model == other.Model;
    }

    #endregion

    private static ObservableCollection<string> _staticBeamColors = new(["Red", "Green", "Blue", "Brown", "White", "Yellow", "Cyan", "Gray", "Orange", "Pink", "Purple"]);

    public float CalculatedPriority => (_model.Enabled ? -1 : 1) * _model.Priority;
    public RuleModel Model => _model;
    private RuleModel _model;

    private readonly Dictionary<NumericFilterType, NumericFilterHelper> _numericHelpers = [];
    private readonly Dictionary<string, NumericFilterHelper> _helperFromString = [];

    [ObservableProperty]
    private ObservableCollection<string> _templates;
    

    public RuleDetailsViewModel(RuleModel rule, ObservableCollection<BlockDetailsViewModel> allBlocks, BlockDetailsViewModel parentBlock)
    {
        if (_templates == null)
        {
            var templateService = App.Current.Services.GetService<RuleTemplateService>();
            _templates = new ObservableCollection<string>(templateService.GetTempalteNames());
        }

        _numericHelpers.Add(NumericFilterType.Stack, new NumericFilterHelper(NumericFilterType.Stack, "Stack Size", "Stack", 5000, (x) => UseStackFilter = x));
        _numericHelpers.Add(NumericFilterType.ItemLevel, new NumericFilterHelper(NumericFilterType.ItemLevel, "Item Level", "ILevel", 100, (x) => UseItemLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.DropLevel, new NumericFilterHelper(NumericFilterType.DropLevel, "Drop Level", "DLevel", 100, (x) => UseDropLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.AreaLevel, new NumericFilterHelper(NumericFilterType.AreaLevel, "Area Level", "ALevel", 100, (x) => UseAreaLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.Quality, new NumericFilterHelper(NumericFilterType.Quality, "Quality", "Quality", 100, (x) => UseQualityFilter = x));
        _numericHelpers.Add(NumericFilterType.Socket, new NumericFilterHelper(NumericFilterType.Socket, "Sockets Count", "Sockets", 4, (x) => UseSocketFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseArmor, new NumericFilterHelper(NumericFilterType.BaseArmor, "Base Armor", "Armor", 5000, (x) => UseArmorFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseEvasion, new NumericFilterHelper(NumericFilterType.BaseEvasion, "Base Evasion", "Evasion", 5000, (x) => UseEvasionFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseEnergyShield, new NumericFilterHelper(NumericFilterType.BaseEnergyShield, "Base Energy Shield", "ES", 5000, (x) => UseESFilter = x));
        _numericHelpers.Add(NumericFilterType.WaystoneTier, new NumericFilterHelper(NumericFilterType.WaystoneTier, "WaystoneTier", "T", 16, (x) => UseWaystoneFilter = x));

        foreach (var value in _numericHelpers.Values)
        {
            _helperFromString[value.Name.Replace(" ", "")] = value;
            _helperFromString[value.ShortName] = value;
        }

        BeamColors = _staticBeamColors;

        IconSizes = new ObservableCollection<string>(["Small", "Medium", "Large"]);
        IconShapes = new(["Circle", "Diamond", "Hexagon", "Square", "Star", "Triangle", "Cross", "Moon", "Raindrop", "Kite"
            , "Pentagon", "UpsideDownHouse"]);

        Sounds = new([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]);


        SetModel(rule);

        AllBlocks = allBlocks;
        SelectedParent = parentBlock;
        _realParent = parentBlock;
    }

    public void SetModel(RuleModel rule)
    {
        _model = rule;
        Modifiers = [new RulePropertiesDecoratorViewModel(this)];
        Title = rule.Title;
        Enabled = rule.Enabled;
        Priority = rule.Priority;
        Show = rule.Show;

        if (rule.FontSize != 0)
        {
            var fontSize = rule.FontSize;
            AddFontSizeModifier();
            FontSize = fontSize;
        }
        else
        {
            FontSize = 32;
        }

        if (rule.TryGetTextColor(out Color textColor))
        {
            UseFontColor = true;
            TextColor = textColor;
        }
        else
        {
            UseFontColor = false;
        }

        if (rule.TryGetBorderColor(out Color borderColor))
        {
            UseBorderColor = true;
            BorderColor = borderColor;
        }
        else
        {
            UseBorderColor = false;
        }
        if (rule.TryGetBackgroundColor(out Color backgroundColor))
        {
            UseBackColor = true;
            BackColor = backgroundColor;
        }
        else
        {
            UseBackColor = false;
        }

        if (UseFontColor || UseBackColor || UseBorderColor)
        {
            AddColorsModifier();
        }
        else
        {
            UseAnyColor = false;
        }
        
        if (rule.Beam != null)
        {
            var color = rule.Beam.Color;
            var lifetime = rule.Beam.IsPermanent;
            AddBeamModifier();
            SelectedBeamColor = color;
            IsBeamPermanent = lifetime;
        }
        else
        {
            UseBeam = false;
            SelectedBeamColor = _staticBeamColors[0];
        }


        if (rule.Icon != null)
        {
            var size = rule.Icon.Size;
            var color = rule.Icon.Color;
            var shape = rule.Icon.Shape;
            AddMinimapIconModifier();
            SelectedIconSize = size;
            SelectedIconColor = color;
            SelectedShape = shape;
        }
        else
        {
            UseMinimapIcon = false;
        }

        if (rule.Sound != null)
        {
            int sample = rule.Sound.Sample;
            int vol = rule.Sound.Volume;
            AddSoundModifier();
            SelectedSound = sample;
            SoundVolume = vol;
        }
        else
        {
            SelectedSound = 0;
            SoundVolume = 300;
        }

        if (rule.TryGetClassCondition(out var classCondition))
        {
            AddClassFilter(classCondition);
        }
        else
        {
            UseClassFilter = false;
        }
        if (rule.TryGetTypeCondition(out var typeCondition))
        {
            AddTypeFilter(typeCondition);
        }
        else
        {
            UseNameFilter = false;
        }

        if (rule.TryGetRarityCondition(out var rarityCondition))
        {
            AddRarityFilter(rarityCondition);
        }
        else
        {
            UseRarityFilter = false;
        }
        foreach (var helper in _numericHelpers.Values)
        {
            helper.Remove();
        }

        foreach (var item in _model.GetNumericConditions())
        {
            AddNumericFilter(item);
        }


        if (rule.TemplateName != null && Templates.Contains(rule.TemplateName))
        {
            SelectedTemplate = rule.TemplateName;
        }
        else
        {
            SelectedTemplate = "Empty";
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

}
