// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    private async Task OnConnectorCreated(IDiagramObject diagramObject)
    {
        var connector = (Connector)diagramObject;

        if (connector.Segments.Count == 0)
        {
            connector.Segments.Add(new BezierSegment());
        }
        else if (connector.Segments[0] is not BezierSegment)
        {
            connector.Segments.Clear();
            connector.Segments.Add(new BezierSegment());
        }

        _connectorManager.ApplyStyle(connector);

        Connection connection;
        if (connector.AdditionalInfo.TryGetValue("Connection", out var connectionObject))
        {
            connection = (Connection)connectionObject;
        }
        else
        {
            var sourceToTarget = connector.SourcePortID == "Out";

            var sourceId = Guid.Parse(sourceToTarget ? connector.SourceID : connector.TargetID);
            var targetId = Guid.Parse(sourceToTarget ? connector.TargetID : connector.SourceID);

            var command = new AddConnectionCommand
            (
                Connection: (diagram, source, target) => new Connection { Diagram = diagram, Source = source, Target = target },
                DiagramId: DiagramId,
                SourceTrendId: sourceId,
                TargetTrendId: targetId
            );
            connection = await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);

            connector.ID = connection.Id.ToString();
            connector.AdditionalInfo["Connection"] = connection;
        }
        _connectorManager.UpdateConnectorFromConnection(connection, connector);
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

        return OnConnectionPointChanged(e.Connector, e.Connector.SourceID, e.Connector.TargetID);
    }

    private Task OnSourceConnectionPointChanged(EndPointChangedEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnSourceConnectionPointChanged));

        return OnConnectionPointChanged(e.Connector, e.Connector.SourceID, e.Connector.TargetID);
    }

    private async Task OnConnectionPointChanged(Connector connector, string sourceId, string targetId)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnConnectionPointChanged));

        if (connector.AdditionalInfo.TryGetValue("Connection", out var connectionObject))
        {
            var connection = (Connection)connectionObject;
            var command = new UpdateConnectionCommand(
                Connection: (source, target) =>
                {
                    _connectorManager.UpdateConnectionFromConnector(connector, connection);
                    connection.Source = source;
                    connection.Target = target;
                    return connection;
                },
                SourceTrendId: Guid.Parse(sourceId),
                TargetTrendId: Guid.Parse(targetId));
            await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);
            _connectorManager.Recalculate(connector);
        }
    }
}
