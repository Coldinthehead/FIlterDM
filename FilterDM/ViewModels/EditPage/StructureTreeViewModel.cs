﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;

public partial class StructureTreeViewModel : ObservableRecipient
    , IRecipient<BlockInFilterCreated>
    , IRecipient<BlockCollectionInFilterChanged>
    , IRecipient<BlockDeletedInFilter>
    , IRecipient<EditorClosedEvent>
    , IRecipient<EditorSelectedEvent>
    , IRecipient<SelectRuleInTreeRequest>
{
    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _blocks;

    [ObservableProperty]
    private ObservableRecipient _selectedNode;

    partial void OnSelectedNodeChanged(ObservableRecipient? oldValue, ObservableRecipient newValue)
    {
        Select(newValue);
    }

    [RelayCommand]
    private void NewBlock()
    {
        Messenger.Send(new CreateBlockRequest(this));
    }

    public StructureTreeViewModel()
    {
        Messenger.Register<BlockInFilterCreated>(this);
        Messenger.Register<BlockCollectionInFilterChanged>(this);
        Messenger.Register<BlockDeletedInFilter>(this);
        Messenger.Register<EditorClosedEvent>(this);
        Messenger.Register<EditorSelectedEvent>(this);
        Messenger.Register<SelectRuleInTreeRequest>(this);
    }

    public void SetBlocks(ObservableCollection<BlockDetailsViewModel> blocks)
    {
        Blocks = blocks;
        ClearSelection();
    }

    public void ClearSelection()
    {
        SelectedNode = null;
    }
    public void Select(ObservableRecipient vm)
    {
        SelectedNode = vm;
        if (vm is BlockDetailsViewModel block)
        {
            Messenger.Send(new BlockSelectedInTree(block));
        }
        else if (vm is RuleDetailsViewModel rule)
        {
            Messenger.Send(new RuleSelectedInTree(rule));
        }
    }

    #region event handlers
    public void Receive(BlockInFilterCreated message)
    {
        Select(message.Value);
    }

    public void Receive(BlockCollectionInFilterChanged message)
    {
        var currentSelection = SelectedNode;
        Blocks = message.Value;
        foreach (var block in Blocks)
        {
            if (currentSelection == block)
            {
                Select(currentSelection);
                break;
            }
            foreach (var rule in block.Rules)
            {
                if (currentSelection == rule)
                {
                    Select(rule);
                    break;
                }
            }
        }
    }

    public void Receive(BlockDeletedInFilter message)
    {
        if (SelectedNode == message.Value)
        {
            ClearSelection();
        }
    }

    public void Receive(EditorClosedEvent message)
    {
        if (SelectedNode == message.Value.GetSelectedContext())
        {
            ClearSelection();
        }
    }

    public void Receive(EditorSelectedEvent message)
    {
        if (SelectedNode != message.Value.GetSelectedContext())
        {
            SelectedNode = message.Value.GetSelectedContext();
        }
    }

    public void Receive(SelectRuleInTreeRequest message)
    {
        Select(message.Value);
    }


    #endregion
}
