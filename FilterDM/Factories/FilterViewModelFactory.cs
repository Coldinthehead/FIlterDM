using FilterDM.Managers;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Managers;

namespace FilterDM.Factories;

public interface IFilterViewModelFactory
{
    public FilterViewModel BuildFilterViewModel();
}

public class FilterViewModelFactory : IFilterViewModelFactory
{
    private readonly ItemTypeService _itemTypeService;
    private readonly BlockTemplateService _blockTemplateService;
    private readonly RuleTemplateService _ruleTemplateService;
    private readonly MinimapIconsService _minimapIconService;
    private readonly SoundService _soundService;

    public FilterViewModelFactory(ItemTypeService itemTypeService
        , BlockTemplateService blockTemplateService
        , RuleTemplateService ruleTemplateService
        , MinimapIconsService minimapIconService
        , SoundService soundService)
    {
        _itemTypeService = itemTypeService;
        _blockTemplateService = blockTemplateService;
        _ruleTemplateService = ruleTemplateService;
        _minimapIconService = minimapIconService;
        _soundService = soundService;
    }

    public FilterViewModel BuildFilterViewModel()
    {
        FilterViewModel vm = new(_itemTypeService
            , new BlockTemplateManager(_blockTemplateService)
            , new RuleTemplateManager(_ruleTemplateService)
            , new PalleteManager()
            , _minimapIconService
            , _soundService
            , new RuleParentManager());
        return vm;
    }

    public BlockDetailsViewModel BuildBlockViewModel()
    {
        return null;
    }

    public RuleDetailsViewModel BuildRuleViewModel()
    {
        return null;
    }
}
