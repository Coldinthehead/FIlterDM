using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FilterDM.Factories;
using FilterDM.Managers;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.Pages;
using FilterDM.Views;
using FilterDM.Views.EditPage;
using FilterDM.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDM;
public partial class App : Application
{
    public new static App? Current => Application.Current as App;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (Program.CrashReport)
            {
                Window window = new ErrorWindow()
                {
                    DataContext = new ErrorViewModel()
                    {
                        Message = Program.CrashExceptionString ?? "Unknown error has occured!."
                    }
                };
                desktop.MainWindow = window;

                base.OnFrameworkInitializationCompleted();
                return;
            }
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            IServiceCollection container = new ServiceCollection();
            var mainVm = new MainWindowViewModel();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = mainVm
            };
            container.AddSingleton<Window>(desktop.MainWindow);
            container.BindService<ItemClassesService>();
            container.BindService<ItemTypeService>();
            container.BindService<FileSelectionService>();
            container.BindService<ProjectRepository>();
            container.BindService<RuleTemplateRepository>();
            container.BindService<DialogService>();
            container.BindService<BlockTemplateRepository>();
            container.BindService<FilterExportService>();
            container.BindService<FilterParserService>();
            container.BindService<BlockTemplateService>();
            container.BindService<RuleTemplateService>();
            container.BindService<ProjectService>();
            container.BindService<FileService>();
            container.BindService<MinimapIconsService>();
            container.BindService<SoundService>();
            container.BindService<FilterViewModelFactory>();
            container.BindService<BlockTemplateManager>();
            container.BindService<RuleTemplateManager>();
            container.BindService<ModifiersFactory>();
            container.BindService<PersistentDataService>();
            Services = container.BuildServiceProvider();

            PersistentDataService data = Services.GetService<PersistentDataService>();
            await data!.InitFolders();
            IEnumerable<IInit> initialiable = Services.GetServices<IInit>();
            List<Task> tasts = new();
            foreach (var i in initialiable)
            {
                tasts.Add(Task.Run(i.Init));
            }
            await Task.WhenAll(tasts);

            mainVm.Initialize(Services);
            mainVm.EnterProjectsPage();

        }

        base.OnFrameworkInitializationCompleted();
    }


    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
