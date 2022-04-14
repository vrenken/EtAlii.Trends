// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;

public partial class TrendMap
{
#pragma warning disable CS8618
    private SfDiagramComponent _diagramComponent;
#pragma warning restore CS8618

    // Specify the orientation of the layout.
    private LayoutOrientation _orientation = LayoutOrientation.TopToBottom;
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Auto;
    private VerticalAlignment _verticalAlignment = VerticalAlignment.Auto;
    private int _horizontalSpacing = 30;
    private int _verticalSpacing = 30;
    private string _diagramHeight = "900px";
    private string _diagramWidth = "100%";
    private readonly DiagramObjectCollection<Node> _nodes = new();
    private readonly DiagramObjectCollection<Connector> _connectors = new();

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
    private void ApplyNodeDefaults(IDiagramObject diagramObject)
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

    private async Task OnNodeTextChanged(TextChangeEventArgs e)
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

    private async Task OnNodePositionChanged(PositionChangedEventArgs e)
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
}
