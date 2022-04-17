// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.ObjectModel;
using EtAlii.Trends.Diagrams;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;
using ClickEventArgs = Syncfusion.Blazor.Diagram.ClickEventArgs;
using Orientation = Syncfusion.Blazor.Diagram.Orientation;

public partial class TrendsDiagram
{
    [Parameter] public Guid DiagramId { get; set; }


    [CascadingParameter(Name = "SelectedTrend")] public Trend? SelectedTrend { get; set; }

    [Parameter] public EventCallback<Trend?> SelectedTrendChanged { get; set; }


    private readonly ObservableCollection<Trend> _trends = new();

    private SfDiagramComponent _trendsDiagram;

    private readonly DiagramObjectCollection<Node> _nodes = new();
    private readonly DiagramObjectCollection<Connector> _connectors = new();

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
                diagram.DiagramTimePosition = _horizontalOffset;
                diagram.DiagramVerticalPosition = _verticalOffset;


                await _commandDispatcher.DispatchAsync<Diagram>(new UpdateDiagramCommand(diagram))
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);
        });
    }

    protected override async Task OnInitializedAsync()
    {
        await InitializePositionAndZoom().ConfigureAwait(false);

        _trends.CollectionChanged += OnTrendsChanged;

        var trends = _queryDispatcher
            .DispatchAsync<Trend>(new GetAllTrendsQuery(DiagramId))
            .ConfigureAwait(false);
        await foreach (var trend in trends)
        {
            _trends.Add(trend);
        }
    }

    // Defines the connector's default values.
    private void ApplyConnectorDefaults(IDiagramObject diagramObject)
    {
        var connector = (Connector)diagramObject;
        connector.Type = ConnectorSegmentType.Orthogonal;
        connector.TargetDecorator.Shape = DecoratorShape.None;
        connector.Style = new ShapeStyle { StrokeColor = "#6d6d6d" };
        connector.Constraints = 0;
        connector.CornerRadius = 5;
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

    // Defines the node's default values.
    private void ApplyTrendNodeDefaults(IDiagramObject diagramObject)
    {
        var node = (Node)diagramObject;
        if (node.Data is System.Text.Json.JsonElement)
        {
            node.Data = System.Text.Json.JsonSerializer.Deserialize<Trend>(node.Data.ToString()!);
        }

        var trend = (Trend)node.Data!;
        node.Style = new ShapeStyle { Fill = "white", StrokeColor = "black", StrokeWidth = 2, };
        node.BackgroundColor = "white";
        node.OffsetX = trend.X;
        node.OffsetY = trend.Y;
        node.Width = trend.W == 0 ? 150 : trend.W;
        node.Height = trend.H == 0 ? 50 : trend.H;
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>
        {
            new()
            {
                Content = trend.Name,
                Style = new TextStyle
                {
                    Color = "black"
                }
            }
        };

        node.Constraints = NodeConstraints.Default &
                           ~NodeConstraints.Rotate &
                           ~NodeConstraints.ResizeNorth &
                           ~NodeConstraints.ResizeNorthWest &
                           ~NodeConstraints.ResizeNorthEast &
                           ~NodeConstraints.ResizeSouth &
                           ~NodeConstraints.ResizeSouthEast &
                           ~NodeConstraints.ResizeSouthWest;
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
        if (e.Count == 2 && e.ActualObject == null)
        {
            await AddNewTrend(e.Position).ConfigureAwait(false);
        }
    }

    private async Task OnTrendSelected(SelectionChangedEventArgs e)
    {
        if (e.NewValue.Count == 1)
        {
            var node = (Node)e.NewValue[0];
            var trend = (Trend)node.Data;

            SelectedTrend = trend;
        }
        else
        {
            SelectedTrend = null;
        }
        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
    }
}
