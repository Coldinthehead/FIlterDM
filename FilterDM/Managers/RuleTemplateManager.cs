using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        Messenger.Send(new ResetRuleTemplateRequest(new ResetRuleTemplateDetails(rule, selectedTemplate)));
    }

    internal int GetIndex(string templateName)
    {
        RuleModel? model = Templates.FirstOrDefault(x => x.Title.Equals(templateName));
        return model == null? 0 : Templates.IndexOf(model);
    }
}