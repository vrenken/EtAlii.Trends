// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class NodeFactory : INodeFactory
{
    public Node Create(Trend trend)
    {

        // if (node.Data is System.Text.Json.JsonElement)
        // {
        //     node.Data = System.Text.Json.JsonSerializer.Deserialize<Trend>(node.Data.ToString()!);
        // }

        // Defines the node's default values.
        var node = new Node {
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
            ID = trend.Id.ToString()
        };

        node.Annotations.Add(new()
        {
            Content = trend.Name,
            Style = new TextStyle
            {
                Color = "black"
            }
        });

        var position = 1f;

        foreach (var component in trend.Components.OrderBy(c => c.Moment))
        {
            position += 0.3f;

            node.Ports.Add(new PointPort
            {
                ID = component.Id.ToString(),
                Shape = PortShapes.Circle,
                Width = 16,
                Height = 16,
                Visibility = PortVisibility.Visible,
                Offset = new DiagramPoint { X = position, Y = 0.5f },
                Style = new ShapeStyle { Fill = "white", StrokeColor = "black" },
                Constraints =  PortConstraints.Draw | PortConstraints.InConnect | PortConstraints.OutConnect
            });

            var characters = component.Name
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s[0])
                .ToArray();
            var name = new string(characters);

            node.Annotations.Add(new ShapeAnnotation
            {
                Constraints = AnnotationConstraints.ReadOnly,
                Visibility = true,
                // Hyperlink = new HyperlinkSettings
                // {
                //     Content = component.Name,
                //     TextDecoration = TextDecoration.None,
                // },
                Style = new TextStyle
                {
                    FontSize = 13,
                    TextWrapping = TextWrap.NoWrap,
                    Bold = false,
                    Color = "black"
                },
                Content = name,
                Offset = new DiagramPoint { X = position - 0.15f, Y = 0.5f },
                Width = 50,
                Height = 50,
            });
        }

        node.Constraints =
            NodeConstraints.ResizeWest |
            NodeConstraints.ResizeEast |
            NodeConstraints.OutConnect |
            NodeConstraints.InConnect |
            NodeConstraints.Delete |
            NodeConstraints.PointerEvents |
            //NodeConstraints.Rotate |
            NodeConstraints.Drag |
            // NodeConstraints.InConnect |
            // NodeConstraints.OutConnect |
            // NodeConstraints.Rotate |
            // NodeConstraints.ResizeNorth |
            // NodeConstraints.ResizeNorthWest |
            // NodeConstraints.ResizeNorthEast |
            // NodeConstraints.ResizeSouth |
            // NodeConstraints.ResizeSouthEast |
            // NodeConstraints.ResizeSouthWest
            NodeConstraints.Select;
        return node;
    }
}
