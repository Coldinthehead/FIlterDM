using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Factories;

public interface IFilterViewModelFactory
{
    public FilterViewModel BuildFilterViewModel();
}

public interface IBlockViewModelFactory
{
    public BlockDetailsViewModel BuildBlockViewModel();
}

public interface IRuleViewModelFactory
{
    public RuleDetailsViewModel BuildRuleViewModel(BlockDetailsViewModel parent, RuleParentManager parentManager, PalleteManager pallateManager);
    public RuleDetailsViewModel BuildRuleViewModel(BlockDetailsViewModel parent, RuleParentManager parentManager, PalleteManager pallateManager, RuleModel model);
}

public class FilterViewModelFactory : IFilterViewModelFactory, IBlockViewModelFactory, IRuleViewModelFactory
{
    private readonly ItemTypeService _itemTypeService;
    private readonly MinimapIconsService _minimapIconService;
    private readonly SoundService _soundService;
    private readonly BlockTemplateManager _blockTemplateManager;
    private readonly RuleTemplateManager _ruleTempalteManager;

    public FilterViewModelFactory(ItemTypeService itemTypeService
        , MinimapIconsService minimapIconService
        , SoundService soundService
        , BlockTemplateManager blockTemplateManager
        , RuleTemplateManager ruleTempalteManager)
    {
        _itemTypeService = itemTypeService;
        _minimapIconService = minimapIconService;
        _soundService = soundService;
        _blockTemplateManager = blockTemplateManager;
        _ruleTempalteManager = ruleTempalteManager;
    }

    public FilterViewModel BuildFilterViewModel()
    {
        FilterViewModel vm = new(
            _blockTemplateManager
            , new PalleteManager()
            , new RuleParentManager()
            , this
            , this);
        return vm;
    }

    public BlockDetailsViewModel BuildBlockViewModel()
    {
        BlockDetailsViewModel block = new(_blockTemplateManager, new TypeScopeManager(_itemTypeService));
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
        RuleDetailsViewModel vm = new(
            parentManager
            , parent.ScopeManager
            , _ruleTempalteManager
            , palleteManager
            , _minimapIconService
            , _soundService);
        vm.SetModel(model);
        return vm;
    }
}
