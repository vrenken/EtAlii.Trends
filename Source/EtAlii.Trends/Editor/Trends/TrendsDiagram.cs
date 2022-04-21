// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.ObjectModel;
using EtAlii.Trends.Diagrams;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;
using ClickEventArgs = Syncfusion.Blazor.Diagram.ClickEventArgs;
using Orientation = Syncfusion.Blazor.Diagram.Orientation;

public partial class TrendsDiagram
{
    [Parameter] public Guid DiagramId { get; set; }
    [CascadingParameter(Name = "SelectedTrend")] public Trend? SelectedTrend { get; set; }

    [Parameter] public EventCallback<Trend?> SelectedTrendChanged { get; set; }

    private SfDiagramComponent _trendsDiagram;

    private readonly DiagramObjectCollection<Node> _nodes = new();
    private readonly DiagramObjectCollection<Connector> _connectors = new();
    private IDiagramObject? DrawingObject => _drawingObjectFactory?.Invoke();
    private Func<IDiagramObject>? _drawingObjectFactory;
    private readonly List<IDiagramObject> _selectedDiagramObjects = new();

#pragma warning disable CS8618
    public TrendsDiagram()
#pragma warning restore CS8618
    {
        _persistPanAndZoom = new ThrottledInvocation(TimeSpan.FromSeconds(2), async () =>
        {
            await InvokeAsync(async () =>
            {
                var diagram = await _queryDispatcher
                    .DispatchAsync<Diagram>(new GetDiagramQuery(DiagramId))
                    .ConfigureAwait(false);

                diagram.DiagramZoom = _currentZoom;
                diagram.HorizontalOffset = _horizontalOffset;
                diagram.VerticalOffset = _verticalOffset;

                await _commandDispatcher.DispatchAsync<Diagram>(new UpdateDiagramCommand(diagram))
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);
        });
    }

    protected override async Task OnInitializedAsync()
    {
        await InitializePositionAndZoom().ConfigureAwait(false);

        await _nodeLoader.Load(_nodes, DiagramId).ConfigureAwait(false);

        await _connectorLoader.Load(_connectors, _nodes, DiagramId).ConfigureAwait(false);

        _connectors.CollectionChanged += OnConnectorsChanged;
    }

    protected override void OnParametersSet()
    {
        if (SelectedTrend != null)
        {
            var node = _nodes.Single(n => n.Data == SelectedTrend);
            _nodeManager.Update(SelectedTrend, node);
            // StateHasChanged();
        }
    }

    // Create the layout info.
    private TreeInfo GetLayoutInfo(IDiagramObject obj, TreeInfo options)
    {
        // Enable the sub-tree.
        options.EnableSubTree = true;
        // Specify the subtree orientation.
        options.Orientation = Orientation.Horizontal;
        return options;
    }


    private async Task OnTrendNodeTextChanged(TextChangeEventArgs e)
    {
        if (e.Element is Node { Data: Trend trend })
        {
            if (!string.IsNullOrWhiteSpace(e.NewValue))
            {
                trend.Name = e.NewValue;

                await _commandDispatcher
                    .DispatchAsync<Trend>(new UpdateTrendCommand(trend))
                    .ConfigureAwait(false);

                SelectedTrend = trend;
                await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
            }
            else
            {
                e.Cancel = true;
            }
        }
    }

    private async Task OnTrendNodePositionChanged(PositionChangedEventArgs e)
    {
        var settings = (DiagramSelectionSettings)e.Element;
        foreach (var node in settings.Nodes)
        {
            var trend = (Trend)node.Data;

            trend.X = node.OffsetX;
            trend.Y = node.OffsetY;

            await _commandDispatcher
                .DispatchAsync<Trend>(new UpdateTrendCommand(trend))
                .ConfigureAwait(false);
        }
    }

    private async Task OnTrendNodeSizeChanged(SizeChangedEventArgs e)
    {
        var settings = e.Element;
        foreach (var node in settings.Nodes)
        {
            var trend = (Trend)node.Data;

            trend.W = node.Width!.Value;
            trend.H = node.Height!.Value;

            await _commandDispatcher
                .DispatchAsync<Trend>(new UpdateTrendCommand(trend))
                .ConfigureAwait(false);
        }
    }

    private async Task OnClicked(ClickEventArgs e)
    {
        switch (e.Count)
        {
            case 2 when e.ActualObject == null:
                await AddNewTrend(e.Position).ConfigureAwait(false);
                break;

            case 1 when e.ActualObject == null:
                _trendsDiagram.ClearSelection();
                break;
        }
    }

    private async Task OnTrendSelected(SelectionChangedEventArgs e)
    {
        SelectedTrend = null;
        _selectedDiagramObjects.Clear();

        var selectedItems = e.NewValue.Count;
        switch (selectedItems)
        {
            case 0: break; // Nothing to do.
            case 1:
                switch (e.NewValue[0])
                {
                    case Node node:
                        var trend = (Trend)node.Data;
                        SelectedTrend = trend;
                        _selectedDiagramObjects.Add(node);
                        break;
                    case Connector connector:
                        _selectedDiagramObjects.Add(connector);
                        break;
                }
                break;
            default: // More than 1 item.
                _selectedDiagramObjects.AddRange(e.NewValue);
                break;
        }

        await SelectedTrendChanged
            .InvokeAsync(SelectedTrend)
            .ConfigureAwait(false);
    }
}
