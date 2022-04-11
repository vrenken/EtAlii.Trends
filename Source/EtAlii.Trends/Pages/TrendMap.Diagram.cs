// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;

public partial class TrendMap
{
    private SfDiagramComponent? _diagram;
    // Specify the layout type.
    private LayoutType _type = LayoutType.HierarchicalTree;
    // Specify the orientation of the layout.
    private LayoutOrientation _orientation = LayoutOrientation.TopToBottom;
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Auto;
    private VerticalAlignment _verticalAlignment = VerticalAlignment.Auto;
    private int _horizontalSpacing = 30;
    private int _verticalSpacing = 30;
    private string _diagramHeight = "900px";


    // Defines the connector's default values.
    private void OnConnectorCreated(IDiagramObject diagramObject)
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
    private void OnNodeCreated(IDiagramObject diagramObject)
    {
        var node = (Node)diagramObject;
        if (node.Data is System.Text.Json.JsonElement)
        {
            node.Data = System.Text.Json.JsonSerializer.Deserialize<Trend>(node.Data.ToString()!);
        }
        var trend = (Trend)node.Data!;
        node.Style = new ShapeStyle { Fill = "#659be5", StrokeColor = "none", StrokeWidth = 2, };
        node.BackgroundColor = "#659be5";
        node.Width = 150;
        node.Height = 50;
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>
        {
            new ShapeAnnotation
            {
                Content = trend.Id,
                Style =new TextStyle {Color = "white"}
            }
        };
    }

    // Create the data source with node name and fill color values.
    private readonly List<Trend> _dataSource = new()
    {
        new( Id: "Diagram", ParentId:""),
        new( Id: "Layout", ParentId: "Diagram"),
        new( Id: "Tree layout", ParentId: "Layout"),
        new( Id: "Organizational chart", ParentId: "Layout"),
        new( Id: "Hierarchical tree", ParentId: "Tree layout"),
        new( Id: "Radial tree", ParentId: "Tree layout"),
        new( Id: "Mind map", ParentId: "Hierarchical tree"),
        new( Id: "Family tree", ParentId: "Hierarchical tree"),
        new( Id: "Management", ParentId: "Organizational chart"),
        new( Id: "Human resources", ParentId: "Management"),
        new( Id: "University", ParentId: "Management"),
        new( Id: "Business", ParentId: "#Management"),
    };
}
