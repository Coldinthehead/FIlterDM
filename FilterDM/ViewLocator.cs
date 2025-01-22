using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels;
using System;
using System.Collections.Generic;

namespace FilterDM;
public class ViewLocator : IDataTemplate
{
    private Dictionary<Type, Control> _cachedViews = [];

    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            //if (_cachedViews.TryGetValue(type, out var view))
            //{
            //    view.DataContext = param;
            //    return view;
            //}

            var control = (Control)Activator.CreateInstance(type)!;
            //_cachedViews.Update(type, control);
            control.DataContext = param;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ObservableObject;
    }
}
