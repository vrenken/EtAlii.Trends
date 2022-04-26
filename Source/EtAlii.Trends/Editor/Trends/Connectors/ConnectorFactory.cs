// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ConnectorFactory : IConnectorFactory
{
    public Connector Create(Connection connection)
    {
        var v1Distance = connection.SourceBezierDistance == 0 ? 50L : connection.SourceBezierDistance;
        var v2Distance = connection.TargetBezierDistance == 0 ? 50L : connection.TargetBezierDistance;

        // Defines the connector's default values.
        var connector = CreateBlank();
        connector.ID = connection.Id.ToString();
        connector.AdditionalInfo = new Dictionary<string, object> { ["Connection"] = connection };
        connector.Segments.Clear();
        connector.Segments.Add(new BezierSegment
        {
            //Defines the Vector1 and Vector2 for the bezier connector.
            Vector1 = new Vector { Distance = v1Distance, Angle = connection.SourceBezierAngle },
            Vector2 = new Vector { Distance = v2Distance, Angle = connection.TargetBezierAngle }
        });
        connector.SourceID = connection.Source.Trend.Id.ToString();
        connector.SourcePortID = connection.Source.Id.ToString();
        connector.TargetID = connection.Target.Trend.Id.ToString();
        connector.TargetPortID = connection.Target.Id.ToString();

        return connector;
    }

    public void ApplyStyle(Connector connector)
    {
        connector.Type = ConnectorSegmentType.Bezier;
        connector.CanAutoLayout = false;
        connector.TargetDecorator = new DecoratorSettings
        {
            Shape = DecoratorShape.Arrow
        };
        connector.Style = new ShapeStyle
        {
            StrokeColor = "#6d6d6d"
        };
        connector.Constraints =
            ConnectorConstraints.ConnectToNearByPort |
            ConnectorConstraints.PointerEvents |
            ConnectorConstraints.InheritBridging |
            ConnectorConstraints.DragTargetEnd |
            ConnectorConstraints.DragSourceEnd |
            //ConnectorConstraints.Drag |
            ConnectorConstraints.Delete |
            //ConnectorConstraints.DragSegmentThumb |
            ConnectorConstraints.Select;

        if (connector.Segments.Count == 0 || connector.Segments[0] is not BezierSegment)
        {
            connector.Segments.Clear();

            var sourceHigherThanTarget = connector.SourcePoint.Y < connector.TargetPoint.Y;
            var delta = sourceHigherThanTarget
                ? connector.TargetPoint.Y - connector.SourcePoint.Y
                : connector.SourcePoint.Y - connector.TargetPoint.Y;
            var sourceBezierAngle = sourceHigherThanTarget ? +90 : -90;
            var sourceBezierDistance = delta / 3f * 2f;
            var targetBezierAngle = sourceHigherThanTarget ? -90 : +90;
            var targetBezierDistance = delta / 3f * 2f;

            connector.Segments.Add(new BezierSegment
            {
                //Defines the Vector1 and Vector2 for the bezier connector.
                Vector1 = new Vector { Distance = sourceBezierDistance, Angle = sourceBezierAngle },
                Vector2 = new Vector { Distance = targetBezierDistance, Angle = targetBezierAngle }
            });

            // Vector1 = new Vector { Distance = 50L, Angle = -90L },
            // Vector2 = new Vector { Distance = 50L, Angle = +90L }

        }
    }
    public Connector CreateBlank()
    {
        // Defines the connector's default values.
        var connector = new Connector
        {
            ID = $"New connector {Guid.NewGuid()}",
        };

        ApplyStyle(connector);

        return connector;
    }
}
