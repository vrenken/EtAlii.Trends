// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class NodeManager : INodeManager
{
    private readonly IPortManager _portManager;

    public NodeManager(IPortManager portManager)
    {
        _portManager = portManager;
    }

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
                NodeConstraints.Select
        };

        node.Annotations.Add(new ShapeAnnotation
        {
            Content = trend.Name,
            Style = new TextStyle
            {
                Color = "black"
            }
        });

        AddOrUpdatePorts(trend, node, out var changed);
        if (changed)
        {
            RepositionPorts(trend, node);
        }

        return node;
    }

    private void RepositionPorts(Trend trend, Node node)
    {
        var position = 1f;

        foreach (var component in trend.Components.OrderBy(c => c.Moment))
        {
            position += 0.3f;
            var port = node.Ports.Single(p => p.ID == component.Id.ToString());
            port.Offset.X = position;
            var annotation = node.Annotations.Single(a => a.ID == component.Id.ToString());
            annotation.Offset.X = position - 0.15f;
        }
    }
    public void Update(Trend trend, Node node)
    {
        AddOrUpdatePorts(trend, node, out var changed);
        if (changed)
        {
            RepositionPorts(trend, node);
        }
    }

    private void AddOrUpdatePorts(Trend trend, Node node, out bool changed)
    {
        var componentsToAdd = trend.Components
            .Where(c => node.Ports.All(p => p.ID != c.Id.ToString()))
            .ToArray();

        var portsToRemove = node.Ports
            .Where(p => trend.Components.All(c => c.Id.ToString() != p.ID))
            .ToArray();

        foreach (var componentToAdd in componentsToAdd)
        {
            var port = _portManager.CreatePort(componentToAdd);
            node.Ports.Add(port);

            var annotation = CreateAnnotation(componentToAdd);
            node.Annotations.Add(annotation);
        }

        foreach (var portToRemove in portsToRemove)
        {
            node.Ports.Remove(portToRemove);

            var annotation = node.Annotations.Single(a => a.ID == portToRemove.ID);
            node.Annotations.Remove(annotation);
        }

        changed = componentsToAdd.Any() || portsToRemove.Any();
    }

    private ShapeAnnotation CreateAnnotation(Component component)
    {
        var characters = component.Name
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s[0])
            .ToArray();
        var name = new string(characters);

        return new ShapeAnnotation
        {
            ID = component.Id.ToString(),
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
            Offset = new DiagramPoint { X = 0f, Y = 0.5f },
            Width = 50,
            Height = 50,
        };
    }
}
