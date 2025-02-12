﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using FilterDM.ViewModels.Base;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Events;

public class BlockInFilterCreated : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockInFilterCreated(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class BlockCollectionInFilterChanged : ValueChangedMessage<ObservableCollection<BlockDetailsViewModel>>
{
    public BlockCollectionInFilterChanged(ObservableCollection<BlockDetailsViewModel> value) : base(value)
    {
    }
}

public class BlockDeletedInFilter : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockDeletedInFilter(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class RuleCreatedInFilter : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleCreatedInFilter(RuleDetailsViewModel value) : base(value)
    {
    }
}

public class BlockSelectedInTree : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockSelectedInTree(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class RuleSelectedInTree : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleSelectedInTree(RuleDetailsViewModel value) : base(value)
    {
    }
}

public class EditorClosedEvent : ValueChangedMessage<EditorBaseViewModel>
{
    public EditorClosedEvent(EditorBaseViewModel value) : base(value)
    {
    }
}

public class EditorSelectedEvent : ValueChangedMessage<EditorBaseViewModel>
{
    public EditorSelectedEvent(EditorBaseViewModel value) : base(value)
    {
    }
}

public class SortBlocksRequest : ValueChangedMessage<BlockDetailsViewModel>
{
    public SortBlocksRequest(BlockDetailsViewModel value) : base(value)
    {
    }
}

public class CreateRuleRequest : ValueChangedMessage<BlockDetailsViewModel>
{
    public CreateRuleRequest(BlockDetailsViewModel value) : base(value)
    {
    }
}
public class MultipleRulesDeleted : ValueChangedMessage<MultipleRuleDeletedDetails>
{
    public MultipleRulesDeleted(MultipleRuleDeletedDetails value) : base(value)
    {
    }
}

public class RuleDeleteEvent : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleDeleteEvent(RuleDetailsViewModel value) : base(value)
    {
    }
}

public class SortRulesRequest : ValueChangedMessage<RuleDetailsViewModel>
{
    public SortRulesRequest(RuleDetailsViewModel value) : base(value)
    {
    }
}

public class SelectRuleInTreeRequest : ValueChangedMessage<RuleDetailsViewModel>
{
    public SelectRuleInTreeRequest(RuleDetailsViewModel value) : base(value)
    {
    }
}
public class ResetRuleTemplateRequest : ValueChangedMessage<ResetRuleTemplateDetails>
{
    public ResetRuleTemplateRequest(ResetRuleTemplateDetails value) : base(value)
    {
    }
}


