// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;

public record MindMapDetails
{
    public string Id { get; init; }
    public string Label { get; init; }
    public string ParentId { get; init; }
    public BranchType Branch { get; init; }
    public string Fill { get; init; }
}
