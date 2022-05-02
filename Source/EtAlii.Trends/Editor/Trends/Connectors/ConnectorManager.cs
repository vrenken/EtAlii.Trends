// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ConnectorManager : IConnectorManager
{
    private readonly ILogger _log = Log.ForContext<ConnectorManager>();

    private readonly ICommandDispatcher _commandDispatcher;

    public ConnectorManager(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public Connector Create(Connection connection)
    {
        // Defines the connector's default values.
        var connector = new Connector
        {
            ID = connection.Id.ToString(),
            Type = ConnectorSegmentType.Bezier,
            AdditionalInfo = new Dictionary<string, object> { ["Connection"] = connection },
            SourceID = connection.Source.Id.ToString(),
            SourcePortID = "Out",
            TargetID = connection.Target.Id.ToString(),
            TargetPortID = "In",
            Segments = new () { new BezierSegment() }
        };

        ApplyStyle(connector);

        return connector;
    }

    public void ApplyStyle(Connector connector)
    {
        connector.Type = ConnectorSegmentType.Bezier;
        connector.CanAutoLayout = false;
        connector.SourceDecorator.Shape = connector.SourcePortID == "In" ? DecoratorShape.Arrow : DecoratorShape.None;
        connector.TargetDecorator.Shape = connector.TargetPortID == "In" ? DecoratorShape.Arrow : DecoratorShape.None;
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
            ConnectorConstraints.ReadOnly |
            ConnectorConstraints.Delete |
            ConnectorConstraints.Select;
    }

    public async Task Recalculate(Node node, DiagramObjectCollection<Connector> connectors)
    {
        foreach (var port in node.Ports)
        {
            var outEdges = connectors
                .Where(c => c.SourceID == node.ID && c.SourcePortID == port.ID)
                .ToArray();
            foreach (var connector in outEdges)
            {
                // connectors.Remove(connector);
                // connectors.Add(connector);
                await Recalculate(connector).ConfigureAwait(false);
            }

            var inEdges = connectors
                .Where(c => c.TargetID == node.ID && c.TargetPortID == port.ID)
                .ToArray();
            foreach (var connector in inEdges)
            {
                // connectors.Remove(connector);
                // connectors.Add(connector);
                await Recalculate(connector).ConfigureAwait(false);
            }
        }
    }

    public async Task Recalculate(Connector connector)
    {
        _log.Verbose("Method called {MethodName}", nameof(Recalculate));

        var sourceLowerThanTarget = connector.SourcePoint.X < connector.TargetPoint.X;
        var delta = Math.Abs(connector.TargetPoint.X - connector.SourcePoint.X);
        var sourceBezierAngle = sourceLowerThanTarget ? 0 : -180;
        var sourceBezierDistance = delta / 3f * 2f;
        var targetBezierAngle = sourceLowerThanTarget ? -180 : 0;
        var targetBezierDistance = delta / 3f * 2f;

        var segment = (BezierSegment)connector.Segments[0];
        segment.Vector1 = new Vector { Distance = sourceBezierDistance, Angle = sourceBezierAngle };
        segment.Vector2 = new Vector { Distance = targetBezierDistance, Angle = targetBezierAngle };

        if (connector.AdditionalInfo.TryGetValue("Connection", out var connectionObject))
        {
            var connection = (Connection)connectionObject;
            var command = new UpdateConnectionCommand(
                Connection: (source, target) =>
                {
                    UpdateConnectionFromConnector(connector, connection);
                    connection.Source = source;
                    connection.Target = target;
                    return connection;
                },
                SourceTrendId: Guid.Parse(connector.SourceID),
                TargetTrendId: Guid.Parse(connector.TargetID));
            await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);
        }
    }

    public void UpdateConnectionFromConnector(Connector connector, Connection connection)
    {
        _log.Verbose("Method called {MethodName}", nameof(UpdateConnectionFromConnector));

        var sourceLowerThanTarget = connector.SourcePoint.X < connector.TargetPoint.X;
        var delta = Math.Abs(connector.TargetPoint.X - connector.SourcePoint.X);
        connection.SourceBezierAngle = sourceLowerThanTarget ? 0 : -180;
        connection.SourceBezierDistance = delta / 3f * 2f;
        connection.TargetBezierAngle = sourceLowerThanTarget ? -180 : 0;
        connection.TargetBezierDistance = delta / 3f * 2f;

    }

    public void UpdateConnectorFromConnection(Connection connection, Connector connector)
    {
        _log.Verbose("Method called {MethodName}", nameof(UpdateConnectorFromConnection));

        var segment = (BezierSegment)connector.Segments[0];
        segment.Vector1.Angle = connection.SourceBezierAngle;
        segment.Vector1.Distance = connection.SourceBezierDistance;
        segment.Vector2.Angle = connection.TargetBezierAngle;
        segment.Vector2.Distance = connection.TargetBezierDistance;

        connector.ID = connection.Id.ToString();
        connector.AdditionalInfo["Connection"] = connection;
    }
}
