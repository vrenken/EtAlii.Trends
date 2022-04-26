// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    public static bool PropagateConnectorUpdates = true;
    private void OnConnectorsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnConnectorsChanged));

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (!PropagateConnectorUpdates)
                {
                    break;
                }
                foreach (Connector connector in e.NewItems!)
                {
                    var command = new AddConnectionCommand
                    (
                        Connection: (diagram, source, target) =>
                        {
                            var connection = new Connection
                            {
                                Diagram = diagram,
                                Source = source,
                                Target = target
                            };
                            UpdateConnectionFromConnector(connector, connection);
                            return connection;
                        },
                        DiagramId: DiagramId,
                        SourceComponentId: Guid.Parse(connector.SourcePortID),
                        TargetComponentId: Guid.Parse(connector.TargetPortID)
                    );
                    var task = _commandDispatcher
                        .DispatchAsync<Connection>(command)
                        .ConfigureAwait(false);

                    var connection = task.GetAwaiter().GetResult();
                    _connectorFactory.ApplyStyle(connector);
                    UpdateConnectorFromConnection(connection, connector);
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
        }
    }

    private Task OnConnectionChanging(ConnectionChangingEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnConnectionChanging));

        switch (e.ConnectorAction)
        {
            case Actions.ConnectorSourceEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                {
                    // No port, let's cancel.
                    e.Cancel = true;
                }
                break;
            case Actions.ConnectorTargetEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID))
                {
                    // No port, let's cancel.
                    e.Cancel = true;

                    if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID) ||
                        string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                    {
                        _connectors.Remove(e.Connector);
                    }
                }
                else
                {

                }
                break;
        }

        return Task.CompletedTask;
    }

    private Task OnConnectionChanged(ConnectionChangedEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnConnectionChanged));

        switch (e.ConnectorAction)
        {
            case Actions.ConnectorSourceEnd:
            case Actions.ConnectorTargetEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID) ||
                    string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                {
                    _connectors.Remove(e.Connector);
                }
                break;
        }
        return Task.CompletedTask;
    }

    private Task OnTargetConnectionPointChanged(EndPointChangedEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnTargetConnectionPointChanged));

        return OnConnectionPointChanged(e.Connector, e.Connector.SourcePortID, e.TargetPortID);
    }

    private Task OnSourceConnectionPointChanged(EndPointChangedEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnSourceConnectionPointChanged));

        return OnConnectionPointChanged(e.Connector, e.TargetPortID, e.Connector.TargetPortID);
    }

    private async Task OnConnectionPointChanged(Connector connector, string sourceComponentId, string targetComponentId)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnConnectionPointChanged));

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
                SourceComponentId: Guid.Parse(sourceComponentId),
                TargetComponentId: Guid.Parse(targetComponentId));
            await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);
        }
    }

    private void UpdateConnectorFromConnection(Connection connection, Connector connector)
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

    private void UpdateConnectionFromConnector(Connector connector, Connection connection)
    {
        _log.Verbose("Method called {MethodName}", nameof(UpdateConnectionFromConnector));

        var sourceHigherThanTarget = connector.SourcePoint.Y < connector.TargetPoint.Y;
        var delta = sourceHigherThanTarget
            ? connector.TargetPoint.Y - connector.SourcePoint.Y
            : connector.SourcePoint.Y - connector.TargetPoint.Y;
        connection.SourceBezierAngle = sourceHigherThanTarget ? +90 : -90;
        connection.SourceBezierDistance = delta / 3f * 2f;
        connection.TargetBezierAngle = sourceHigherThanTarget ? -90 : +90;
        connection.TargetBezierDistance = delta / 3f * 2f;

        // The below angle and distances do not change. Feels weird.
        // var segment = (BezierSegment)connector.Segments[0];
        // connection.SourceBezierAngle = segment.Vector1.Angle;
        // connection.SourceBezierDistance = segment.Vector1.Distance;
        // connection.TargetBezierAngle = segment.Vector2.Angle;
        // connection.TargetBezierDistance = segment.Vector2.Distance;

    }
}
