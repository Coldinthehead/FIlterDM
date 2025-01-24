using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Factories;

public class FilterViewModelFactory : IFilterViewModelFactory, IBlockViewModelFactory, IRuleViewModelFactory
{
    private readonly ItemTypeService _itemTypeService;
    private readonly MinimapIconsService _minimapIconService;
    private readonly SoundService _soundService;
    private readonly BlockTemplateManager _blockTemplateManager;
    private readonly RuleTemplateManager _ruleTempalteManager;
    private readonly DialogService _dialogSevice;
    private readonly IModifiersFacotry _modifiersFactory;

    public FilterViewModelFactory(ItemTypeService itemTypeService
        , MinimapIconsService minimapIconService
        , SoundService soundService
        , BlockTemplateManager blockTemplateManager
        , RuleTemplateManager ruleTempalteManager
        , DialogService dialogSevice
        , IModifiersFacotry modifierFactory)
    {
        _itemTypeService = itemTypeService;
        _minimapIconService = minimapIconService;
        _soundService = soundService;
        _blockTemplateManager = blockTemplateManager;
        _ruleTempalteManager = ruleTempalteManager;
        _dialogSevice = dialogSevice;
        _modifiersFactory = modifierFactory;
    }

    public FilterViewModel BuildFilterViewModel()
    {
        FilterViewModel vm = new(new PalleteManager()
            , new RuleParentManager()
            , this
            , this);
        return vm;
    }

    public BlockDetailsViewModel BuildBlockViewModel()
    {
        BlockDetailsViewModel block = new(_blockTemplateManager, new TypeScopeManager(_itemTypeService), _dialogSevice);
        BlockModel template = _blockTemplateManager.GetEmpty();
        block.SetModel(template);
        return block;
    }

    public RuleDetailsViewModel BuildRuleViewModel(
        BlockDetailsViewModel parent
        , RuleParentManager parentManager
        , PalleteManager palleteManager)
    {
        return BuildRuleViewModel(parent, parentManager, palleteManager, _ruleTempalteManager.GetEmpty());
    }

    public RuleDetailsViewModel BuildRuleViewModel(
      BlockDetailsViewModel parent
      , RuleParentManager parentManager
      , PalleteManager palleteManager
      , RuleModel model)
    {
        RuleDetailsViewModel vm = new(parentManager
            , parent.ScopeManager
            , _ruleTempalteManager
            , palleteManager
            , _dialogSevice
            , _modifiersFactory);
        vm.SetModel(model);
        return vm;
    }
}
