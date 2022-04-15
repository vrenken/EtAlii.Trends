// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram;

public partial class TrendsDiagram
{
    [Parameter] public Guid DiagramId { get; set; }


    [Parameter] public Trend? Trend { get; set; }


    [Parameter] public EventCallback<Trend?> TrendChanged { get; set; }


    private readonly ObservableCollection<Trend> _trends = new();

    #pragma warning disable CS8618
    private SfDiagramComponent _trendsDiagram;
    #pragma warning restore CS8618

    private readonly DiagramObjectCollection<Node> _nodes = new();
    private readonly DiagramObjectCollection<Connector> _connectors = new();

    protected override async Task OnInitializedAsync()
    {
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
        node.Width = 150;
        node.Height = 50;
        node.OffsetX = trend.X;
        node.OffsetY = trend.Y;
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
        node.Constraints = NodeConstraints.Default & ~NodeConstraints.Rotate;
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

            Trend = trend;
        }
        else
        {
            Trend = null;
        }
        await TrendChanged.InvokeAsync(Trend).ConfigureAwait(false);
    }
}