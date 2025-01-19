using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using System.Collections.Generic;

namespace FilterDM.Managers;

public class TypeScopeManager
{
    private List<TypeListViewModel> _scopeNames;

    private List<TypeDecoratorViewModel> _decorators = [];

    private bool _useScope = false;

    private readonly ItemTypeService _typeService;

    public TypeScopeManager(ItemTypeService typeService)
    {
        _typeService = typeService;
        _scopeNames = typeService.BuildEmptyList();
    }

    public void DisableScope()
    {
        _useScope = false;
        foreach (var decorator in _decorators)
        {
            decorator.ReleaseScope(_typeService.BuildEmptyList());
        }
    }

    public void EnableScope()
    {
        _useScope = true;
        foreach (var decorator in _decorators)
        {
            decorator.SetScoped(_scopeNames);
        }
    }

    public void RemoveByRule(RuleDetailsViewModel rule)
    {
        foreach (var d in _decorators)
        {
            if (d.Rule == rule)
            {
                _decorators.Remove(d);
                d.ReleaseScope(_typeService.BuildEmptyList());
                break;
            }
        }
    }

    public void AddByExistingRule(RuleDetailsViewModel rule)
    {
        foreach (var mod in rule.Modifiers)
        {
            if (mod is TypeDecoratorViewModel mode)
            {
                _decorators.Add(mode);
                mode.DeleteAction = RemoveDecatorator;
                if (_useScope)
                {
                    mode.SetScoped(_scopeNames);
                }
            }
        }
    }

    public TypeDecoratorViewModel GetDecorator(RuleDetailsViewModel vm)
    {
        TypeDecoratorViewModel decorator = new(vm, RemoveDecatorator);
        _decorators.Add(decorator);

        if (_useScope)
        {
            decorator.SetScoped(_scopeNames);
        }
        else
        {
            decorator.InitalizeFromEmpty(_typeService.BuildEmptyList());
        }
        return decorator;
    }


    private void RemoveDecatorator(ModifierViewModelBase vm)
    {
        if (vm is TypeDecoratorViewModel model)
        {
            vm.Rule.RemoveTypeFilter(model);
            _decorators.Remove(model);
            if (_useScope)
            {
                model.ReleaseScope(_typeService.BuildEmptyList());
            }
        }
    }
}
