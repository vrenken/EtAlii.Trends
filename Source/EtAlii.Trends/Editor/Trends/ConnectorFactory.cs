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
        connector.Segments = new DiagramObjectCollection<ConnectorSegment>
            {
                new BezierSegment
                {
                    //Defines the Vector1 and Vector2 for the bezier connector.
                    Vector1 = new Vector { Distance = v1Distance, Angle = connection.SourceBezierAngle },
                    Vector2 = new Vector { Distance = v2Distance, Angle = connection.TargetBezierAngle }
                }
            };
        connector.SourceID = connection.Source.Trend.Id.ToString();
        connector.SourcePortID = connection.Source.Id.ToString();
        connector.TargetID = connection.Target.Trend.Id.ToString();
        connector.TargetPortID = connection.Target.Id.ToString();

        return connector;
    }

    public Connector CreateBlank()
    {
        // Defines the connector's default values.
        var connector = new Connector
        {
            ID = $"New connector {Guid.NewGuid()}",
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
            Segments = new DiagramObjectCollection<ConnectorSegment>
            {
                new BezierSegment
                {
                    //Defines the Vector1 and Vector2 for the bezier connector.
                    Vector1 = new Vector { Distance = 50L, Angle = -90L },
                    Vector2 = new Vector { Distance = 50L, Angle = +90L }
                }
            },
            CanAutoLayout = false,
        };

        return connector;
    }
}
