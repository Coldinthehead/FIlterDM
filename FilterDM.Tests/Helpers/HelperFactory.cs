using FilterDM.Factories;
using FilterDM.Managers;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.Pages;

namespace FilterDM.Tests.Helpers;
public static class HelperFactory
{
    public static FilterViewModel GetFilter(CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger messenger)
    {
        return new FilterViewModel(messenger,
            new PalleteManager()
            , new RuleParentManager()
            , GetFactoryInstance()
            , GetFactoryInstance());
    }
    public static FilterViewModel GetFilter()
    {
        FilterViewModel filter = new FilterViewModel(
            new PalleteManager()
            , new RuleParentManager()
            , GetFactoryInstance()
            , GetFactoryInstance()
            );
        return filter;
    }
    public static BlockDetailsViewModel GetBlock()
    {
        return GetFactoryInstance().BuildBlockViewModel();
    }


    private static FilterViewModelFactory _factoryInstance;
    public static FilterViewModelFactory GetFactoryInstance()
    {
        if ( _factoryInstance == null )
        {
            _factoryInstance = new(
                new Services.ItemTypeService(),
                new Services.MinimapIconsService(),
                new Services.SoundService(),
                new BlockTemplateManager(new Services.BlockTemplateService(new BlockTemplateRepository(new PersistentDataService()))),
                new RuleTemplateManager(new RuleTemplateService(new RuleTemplateRepository(new PersistentDataService())))
                , new DialogService(null)
                , new ModifiersFactory(new MinimapIconsService(), new SoundService()));

        }
        return _factoryInstance;
    }

    internal static RuleDetailsViewModel GetRule(BlockDetailsViewModel testBlock)
    {
        return GetFactoryInstance().BuildRuleViewModel(testBlock, new RuleParentManager(), new PalleteManager());
    }

    public static ProjectEditViewModel GetProject()
    {
        return new ProjectEditViewModel(new ItemTypeService()
            , new ProjectService(new ProjectRepository(new FileService(new DialogService(null)), new PersistentDataService())
            , new DialogService(null))
            , new FileSelectionService(null)
            , new FileService(new DialogService(null))
            , new FilterExportService()
            , GetFactoryInstance());
    }
}
