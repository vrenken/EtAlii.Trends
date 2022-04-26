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

    public Node Create(Trend trend, DiagramObjectCollection<Connector> connectors)
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
                NodeConstraints.Select
        };

        var annotation = CreateAnnotation(trend.Name);
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>(new [] { annotation });

        Update(trend, node, connectors);

        return node;
    }

    public void Update(Trend trend, Node node, DiagramObjectCollection<Connector> connectors)
    {
        var position = node.Width!.Value;

        var margin = 13D;
        var characterWidth = 10D;

        var visibleAnnotations = new List<ShapeAnnotation>(new [] { node.Annotations.First() });
        var visiblePorts = new List<PointPort>();

        foreach (var component in trend.Components.OrderBy(c => c.Moment))
        {
            var annotation = GetOrAddAnnotation(component, node);
            visibleAnnotations.Add(annotation);

            var port = GetOrAddPort(component, node);
            visiblePorts.Add(port);

            var nextPortOffset = characterWidth * annotation.Content.Length + 2 * margin;
            var nextAnnotationOffset = nextPortOffset / 2D;

            annotation.Margin = new Margin { Left = position + nextAnnotationOffset };
            port.Margin = new Margin { Left = position + nextPortOffset};

            position += nextPortOffset;
        }

        var portsToRemove = node.Ports.Except(visiblePorts).ToArray();
        foreach (var portToRemove in portsToRemove)
        {
            node.Ports.Remove(portToRemove);
        }

        var annotationsToRemove = node.Annotations.Except(visibleAnnotations).ToArray();
        foreach (var annotationToRemove in annotationsToRemove)
        {
            node.Annotations.Remove(annotationToRemove);
        }

        node.Ports = new DiagramObjectCollection<PointPort>(node.Ports);
        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>(node.Annotations);

        TrendsDiagram.PropagateConnectorUpdates = false;
        foreach (var port in node.Ports)
        {
            var outEdges = connectors
                .Where(c => c.SourceID == node.ID && c.SourcePortID == port.ID)
                .ToArray();
            foreach (var connector in outEdges)
            {
                connectors.Remove(connector);
                connectors.Add(connector);
            }

            var inEdges = connectors
                .Where(c => c.TargetID == node.ID && c.TargetPortID == port.ID)
                .ToArray();
            foreach (var connector in inEdges)
            {
                connectors.Remove(connector);
                connectors.Add(connector);
            }
        }
        TrendsDiagram.PropagateConnectorUpdates = true;
    }

    private ShapeAnnotation GetOrAddAnnotation(Component component, Node node)
    {
        var annotation = node.Annotations.SingleOrDefault(a => a.ID == $"{component.Id}_Annotation");
        if (annotation == null)
        {
            annotation = CreateAnnotation(component);
            node.Annotations.Add(annotation);
        }
        return annotation;
    }

    private PointPort GetOrAddPort(Component component, Node node)
    {
        var port = node.Ports.SingleOrDefault(a => a.ID == $"{component.Id}");
        if (port == null)
        {
            port = _portManager.CreatePort(component);
            node.Ports.Add(port);
        }
        return port;
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
