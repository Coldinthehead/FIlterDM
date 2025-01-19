using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.Managers;

public partial class BlockTemplateManager : ViewModelBase
{
    private readonly BlockTemplateService _blockTemplateService;

    [ObservableProperty]
    public ObservableCollection<BlockModel> _templates;

    public BlockTemplateManager(BlockTemplateService blockTemplateService)
    {
        _blockTemplateService = blockTemplateService;
        Templates = new (blockTemplateService.GetTemplates());
    }

    internal BlockModel GetEmpty() => throw new NotImplementedException();
    internal bool HasTemplate(string templateName) => throw new NotImplementedException();
}
