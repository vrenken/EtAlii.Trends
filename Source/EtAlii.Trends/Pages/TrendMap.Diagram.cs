// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;

public partial class TrendMap
{
    private SfDiagramComponent? _diagram;
    // Specify the layout type.
    private LayoutType _type = LayoutType.MindMap;
    // Specify the orientation of the layout.
    private LayoutOrientation _orientation = LayoutOrientation.TopToBottom;
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Auto;
    private VerticalAlignment _verticalAlignment = VerticalAlignment.Auto;
    private int _horizontalSpacing = 30;
    private int _verticalSpacing = 30;
    private string _height = "900px";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var windowDimensions = await JsRuntime
            .InvokeAsync<WindowDimension>("getWindowDimensions", CancellationToken.None, null)
            .ConfigureAwait(false);

        _height = $"{windowDimensions.Height}px";
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }


    public class WindowDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }


    // Defines the connector's default values.
    private void ConnectorDefaults(IDiagramObject diagramObject)
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
    private void NodeDefaults(IDiagramObject obj)
    {
        var node = (Node)obj;
        if (node.Data is System.Text.Json.JsonElement)
        {
            node.Data = System.Text.Json.JsonSerializer.Deserialize<HierarchicalDetails>(node.Data.ToString()!);
        }
        var hierarchicalData = (HierarchicalDetails)node.Data!;
        node.Style = new ShapeStyle { Fill = "#659be5", StrokeColor = "none", StrokeWidth = 2, };
        node.BackgroundColor = "#659be5";
        node.Width = 150;
        node.Height = 50;
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>
        {
            new ShapeAnnotation
            {
                Content = hierarchicalData.Name,
                Style =new TextStyle {Color = "white"}
            }
        };
    }

    // Create the data source with node name and fill color values.
    private readonly List<HierarchicalDetails> _dataSource = new()
    {
        new( Name :"Diagram", Category:"",FillColor:"#659be5"),
        new( Name :"Layout", Category:"Diagram",FillColor:"#659be5"),
        new( Name :"Tree layout", Category:"Layout",FillColor:"#659be5"),
        new( Name :"Organizational chart", Category:"Layout",FillColor:"#659be5"),
        new( Name :"Hierarchical tree", Category:"Tree layout",FillColor:"#659be5"),
        new( Name :"Radial tree", Category:"Tree layout",FillColor:"#659be5"),
        new( Name :"Mind map", Category:"Hierarchical tree",FillColor:"#659be5"),
        new( Name :"Family tree", Category:"Hierarchical tree",FillColor:"#659be5"),
        new( Name :"Management", Category:"Organizational chart",FillColor:"#659be5"),
        new( Name :"Human resources", Category:"Management",FillColor:"#659be5"),
        new( Name :"University", Category:"Management",FillColor:"#659be5"),
        new( Name :"Business", Category:"#Management",FillColor:"#659be5")
    };
}
