using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;

public partial class EditorPanelViewModel : ObservableRecipient
    , IRecipient<BlockInFilterCreated>
    , IRecipient<BlockDeletedInFilter>
{
    [ObservableProperty]
    private ObservableCollection<EditorBaseViewModel> _items;

    [ObservableProperty]
    private EditorBaseViewModel _selectedItem;
    partial void OnSelectedItemChanged(EditorBaseViewModel? oldValue, EditorBaseViewModel newValue)
    {
        if (newValue is RuleEditorViewModel r)
        {
            r.Title = r.Rule.Properties.Title;
            Messenger.Send(new RuleSelectedRequestEvent(r.Rule));
        }
        else if (newValue is BlockEditorViewModel b)
        {
            b.Title = b.Block.Title;
            Messenger.Send(new BlockSelectedRequestEvent(b.Block));
        }
    }

    public void AddEditor(EditorBaseViewModel editor)
    {
        foreach (var item in Items)
        {
            if (item.Equals(editor))
            {
                SelectedItem = editor;
                return;
            }
        }
        Items.Add(editor);
        SelectedItem = editor;
    }

    public void AddRule(RuleDetailsViewModel vm)
    {
        RuleEditorViewModel editor = new(vm);
        editor.CloseAction = CloseTab;
        
        this.AddEditor(editor);
    }

    public void AddRuleDontSelect(RuleDetailsViewModel vm)
    {
        RuleEditorViewModel editor = new(vm);
        editor.CloseAction = CloseTab;

        foreach (var item in Items)
        {
            if (item.Equals(editor))
            {
                return;
            }
        }
        Items.Add(editor);
       
    }
    public void AddBlock(BlockDetailsViewModel vm)
    {
        BlockEditorViewModel editor = new(vm);
        editor.CloseAction = CloseTab;
        this.AddEditor(editor);  
    }

    public void CloseTab(EditorBaseViewModel vm)
    {
        Items.Remove(vm);

        if (vm is RuleEditorViewModel r)
        {
            Messenger.Send(new RuleCloseRequestEvent(r));
        }
        else if (vm is BlockEditorViewModel b)
        {
            Messenger.Send(new BlockCloseRequestEvent(b));
        }
    }

    public void CloseRulesFromBlock(BlockDetailsViewModel block)
    {
        List<EditorBaseViewModel> next = [];
        foreach (var openTab in Items)
        {
            if (!openTab.IsPartOf(block))
            {
                next.Add(openTab);
            }
        }
        Items = new(next);
    }

    internal void CloseRule(RuleDetailsViewModel value)
    {
        EditorBaseViewModel? editor = null;
        foreach (var openTab in Items)
        {
            if (openTab is RuleEditorViewModel vm && vm.Rule == value)
            {
                editor = openTab;
                break;
            }
        }
        if (editor != null)
        {
            Items.Remove(editor);
        }
    }

    public void Clear()
    {
        Items.Clear();
    }


    public EditorPanelViewModel()
    {
        Items = new();
        Messenger.Register<BlockInFilterCreated>(this);
        Messenger.Register<BlockDeletedInFilter>(this);
    }

    #region event handlers
    public void Receive(BlockInFilterCreated message)
    {
        AddBlock(message.Value);
    }

    public void Receive(BlockDeletedInFilter message)
    {
        CloseRulesFromBlock(message.Value);
    }

    #endregion
}
