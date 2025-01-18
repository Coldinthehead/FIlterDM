using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;

public partial class EditorPanelViewModel : ObservableRecipient
    , IRecipient<BlockInFilterCreated>
    , IRecipient<BlockDeletedInFilter>
    , IRecipient<BlockSelectedInTree>
    , IRecipient<RuleSelectedInTree>
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
        }
        else if (newValue is BlockEditorViewModel b)
        {
            b.Title = b.Block.Title;
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
        this.AddEditor(editor);
    }

    public void AddBlock(BlockDetailsViewModel vm)
    {
        BlockEditorViewModel editor = new(vm);
        this.AddEditor(editor);  
    }

    public void CloseTab(EditorBaseViewModel vm)
    {
        Items.Remove(vm);
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
        Messenger.Register<BlockSelectedInTree>(this);
        Messenger.Register<RuleSelectedInTree>(this);
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

    public void Receive(BlockSelectedInTree message)
    {
        AddBlock(message.Value);
    }

    public void Receive(RuleSelectedInTree message)
    {
        AddRule(message.Value);
    }

    #endregion
}
