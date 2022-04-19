// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Reflection;
using Syncfusion.Blazor.Diagram;

public class ConnectorFactory : IConnectorFactory
{
    private static readonly PropertyInfo _bezierPoint2Property;
    private static readonly PropertyInfo _bezierPoint1Property;
    private static readonly PropertyInfo _pointsProperty;
    private static readonly MethodInfo _setParentMethod;

    static ConnectorFactory()
    {
        var type = typeof(BezierSegment);
        _bezierPoint2Property = type.GetProperty("BezierPoint2", BindingFlags.Instance | BindingFlags.NonPublic)!;
        _bezierPoint1Property = type.GetProperty("BezierPoint1", BindingFlags.Instance | BindingFlags.NonPublic)!;
        _pointsProperty = type.GetProperty("Points", BindingFlags.Instance | BindingFlags.NonPublic)!;
        _setParentMethod = type.GetMethod("SetParent", BindingFlags.Instance | BindingFlags.NonPublic)!;

    }

    public Connector Create(Connection connection)
    {
        // var sourcePoint = nodes
        //     .Where(n => n.ID == connection.Source.Trend.Id.ToString())
        //     .SelectMany(n => n.Ports)
        //     .Where(n => n.ID == connection.Source.Id.ToString())
        //     .Select(n => n.Offset)
        //     .Single();
        //
        // var targetPoint = nodes
        //     .Where(n => n.ID == connection.Target.Trend.Id.ToString())
        //     .SelectMany(n => n.Ports)
        //     .Where(n => n.ID == connection.Target.Id.ToString())
        //     .Select(n => n.Offset)
        //     .Single();

        var segment = new BezierSegment();

        var bezierPoint1 = new DiagramPoint(connection.SourceBezierX, connection.SourceBezierY);
        _bezierPoint1Property.SetValue(segment, bezierPoint1);

        var bezierPoint2 = new DiagramPoint(connection.TargetBezierX, connection.TargetBezierY);
        _bezierPoint2Property.SetValue(segment, bezierPoint2);

        // var points = new List<DiagramPoint>( new [] { sourcePoint, targetPoint });
        // _pointsProperty.SetValue(segment, points);


        // Defines the connector's default values.
        var connector = new Connector
        {
            Type = ConnectorSegmentType.Bezier,
            TargetDecorator =
            {
                Shape = DecoratorShape.None
            },
            Style = new ShapeStyle
            {
                StrokeColor = "#6d6d6d"
            },
            Constraints = ConnectorConstraints.Default,
            AdditionalInfo = new Dictionary<string, object> { ["Connection"] = connection },
            Segments = new DiagramObjectCollection<ConnectorSegment> { segment },
            CanAutoLayout = false,
            ID = connection.Id.ToString(),
            SourceID = connection.Source.Trend.Id.ToString(),
            SourcePortID = connection.Source.Id.ToString(),
            TargetID = connection.Target.Trend.Id.ToString(),
            TargetPortID = connection.Target.Id.ToString(),
        };

        return connector;
    }
}
