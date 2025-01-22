using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.Managers;

public partial class BlockTemplateManager : ObservableRecipient
{
    private readonly BlockTemplateService _blockTemplateService;

    [ObservableProperty]
    public ObservableCollection<BlockModel> _templates;

    public BlockTemplateManager(BlockTemplateService blockTemplateService)
    {
        _blockTemplateService = blockTemplateService;
        Templates = new (blockTemplateService.GetTemplates());
    }

    public BlockModel GetEmpty() => _blockTemplateService.GetEmpty();
    public BlockModel GetTemplate(string templateName) => _blockTemplateService.GetTemplate(templateName);

    public int GetSelectionIndex(string name)
    {
        BlockModel? model = Templates.First(x => x.Title.Equals(name));
        if (model != null)
        {
            return Templates.IndexOf(model);
        }
        return 0;
    }
    public bool HasTemplate(string templateName) => _blockTemplateService.HasTemplate(templateName);
    public void SetTempalte(BlockDetailsViewModel blockDetailsViewModel, BlockModel selectedTemplate)
    {
        Messenger.Send(new ResetTemplateRequest(new TemplateChangeDetils(blockDetailsViewModel, selectedTemplate)));
    }
}
