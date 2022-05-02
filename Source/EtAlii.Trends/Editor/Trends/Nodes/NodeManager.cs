// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class NodeManager : INodeManager
{
    public Node Create(Trend trend)
    {
        // Defines the node's default values.
        var node = new Node
        {
            Data = trend,
            Style = new ShapeStyle
            {
                Fill = "white",
                StrokeColor = "black",
                StrokeWidth = 2,
            },
            BackgroundColor = "white",
            OffsetX = trend.X,
            OffsetY = trend.Y,
            Width = trend.W == 0 ? 150 : trend.W,
            Height = trend.H == 0 ? 50 : trend.H,
            ID = trend.Id.ToString(),
            Constraints =
                NodeConstraints.ResizeWest |
                NodeConstraints.ResizeEast |
                NodeConstraints.Delete |
                NodeConstraints.PointerEvents |
                NodeConstraints.Drag |
                NodeConstraints.Select,
            Annotations = new DiagramObjectCollection<ShapeAnnotation>
            {
                new() { Content = trend.Name, Style = new TextStyle { Color = "black" } }
            },
            Ports = new DiagramObjectCollection<PointPort>
            {
                new()
                {
                    ID = "In",
                    Shape = PortShapes.Circle,
                    Width = 16,
                    Height = 16,
                    Visibility = PortVisibility.Visible,
                    Offset = new DiagramPoint { X = 0f, Y = 0.5f },
                    Style = new ShapeStyle { Fill = "white", StrokeColor = "black" },
                    Constraints = PortConstraints.Default
                },
                new()
                {
                    ID = "Out",
                    Shape = PortShapes.Circle,
                    Width = 16,
                    Height = 16,
                    Visibility = PortVisibility.Visible,
                    Offset = new DiagramPoint { X = 1f, Y = 0.5f },
                    Style = new ShapeStyle { Fill = "white", StrokeColor = "black" },
                    Constraints = PortConstraints.Default
                }
            }
        };
        return node;
    }
}
