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

        var annotation = CreateAnnotation(trend.Name);
        node.Annotations.Add(annotation);

        AddOrUpdatePorts(trend, node, out var changed);
        if (changed)
        {
            RepositionPorts(trend, node);
        }

        return node;
    }

    private void RepositionPorts(Trend trend, Node node)
    {
        var position = node.Width!.Value;

        var margin = 13D;
        var characterWidth = 10D;
        foreach (var component in trend.Components.OrderBy(c => c.Moment))
        {
            var annotation = node.Annotations.Single(a => a.ID == $"{component.Id}_Annotation");
            var port = node.Ports.Single(p => p.ID == component.Id.ToString());
            var nextPortOffset = characterWidth * annotation.Content.Length + 2 * margin;
            var nextAnnotationOffset = nextPortOffset / 2D;

            annotation.Margin = new Margin { Left = position + nextAnnotationOffset };
            port.Margin = new Margin { Left = position + nextPortOffset};

            position += nextPortOffset;
        }

        node.Ports = new DiagramObjectCollection<PointPort>(node.Ports);
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>(node.Annotations);
    }
    public void Update(Trend trend, Node node, out bool changed)
    {
        AddOrUpdatePorts(trend, node, out changed);
        if (changed)
        {
            RepositionPorts(trend, node);
        }
    }

    private void AddOrUpdatePorts(Trend trend, Node node, out bool changed)
    {
        changed = false;
        var portsToAdd = trend.Components
            .Where(c => node.Ports.All(p => p.ID != c.Id.ToString()))
            .ToArray();

        var portsToRemove = node.Ports
            .Where(p => trend.Components.All(c => c.Id.ToString() != p.ID))
            .ToArray();
        var unchangedPorts = node.Ports.Except(portsToRemove).ToArray();

        if (portsToAdd.Any() || portsToRemove.Any())
        {
            foreach (var portToRemove in portsToRemove)
            {
                node.Ports.Remove(portToRemove);
            }
            var visiblePorts = new DiagramObjectCollection<PointPort>(unchangedPorts);
            foreach (var portToAdd in portsToAdd)
            {
                var port = _portManager.CreatePort(portToAdd);
                visiblePorts.Add(port);
            }
            node.Ports = visiblePorts;

            changed = true;
        }

        var annotationsToAdd = trend.Components
            .Where(c => node.Annotations.All(a => a.ID != $"{c.Id}_Annotation"))
            .ToArray();

        var annotationsToRemove = node.Annotations
            .Skip(1)
            .Where(a => trend.Components.All(c => $"{c.Id}_Annotation" != a.ID))
            .ToArray();
        var unchangedAnnotations = node.Annotations.Except(annotationsToRemove).ToArray();

        if (annotationsToAdd.Any() || annotationsToRemove.Any())
        {
            foreach (var annotationToRemove in annotationsToRemove)
            {
                node.Annotations.Remove(annotationToRemove);
            }
            var visibleAnnotations = new DiagramObjectCollection<ShapeAnnotation>(unchangedAnnotations);
            foreach (var annotationToAdd in annotationsToAdd)
            {
                var annotation = CreateAnnotation(annotationToAdd);
                //node.Annotations.Add(annotation);
                visibleAnnotations.Add(annotation);
            }
            node.Annotations = visibleAnnotations;

            changed = true;
        }
    }

    private ShapeAnnotation CreateAnnotation(string name)
    {
        return new ShapeAnnotation { Content = name, Style = new TextStyle { Color = "black" } };
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
            AdditionalInfo = { ["Component"] = component },
            ID = $"{component.Id}_Annotation",
            Constraints = AnnotationConstraints.ReadOnly,
            Visibility = true,
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
