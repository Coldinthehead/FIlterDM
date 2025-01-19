using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.Services;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Managers;

public partial class RuleTemplateManager : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<RuleModel> _templates;
    
    private readonly IRuleTemplateService _ruleTemplateService;

    public RuleTemplateManager(IRuleTemplateService ruleTemplateService)
    {
        _ruleTemplateService = ruleTemplateService;
        _templates = new (_ruleTemplateService.GetAll());
    }

    internal RuleModel Get(string templateName) => _ruleTemplateService.Get(templateName);
    internal RuleModel GetEmpty() => _ruleTemplateService.GetEmpty();
    public void SetTemplate(RuleDetailsViewModel rule, RuleModel selectedTemplate)
    {
        throw new NotImplementedException();
    }
}