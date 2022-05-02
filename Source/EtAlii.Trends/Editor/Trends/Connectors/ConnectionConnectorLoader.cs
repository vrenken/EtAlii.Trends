// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ConnectionConnectorLoader : IConnectionConnectorLoader
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IConnectorManager _connectorManager;

    public ConnectionConnectorLoader(IQueryDispatcher queryDispatcher, IConnectorManager connectorManager)
    {
        _queryDispatcher = queryDispatcher;
        _connectorManager = connectorManager;
    }

    public async Task Load(DiagramObjectCollection<Connector> connectors, DiagramObjectCollection<Node> nodes, Guid diagramId)
    {
        var connections = _queryDispatcher
            .DispatchAsync<Connection>(new GetAllConnectionsQuery(diagramId))
            .ConfigureAwait(false);

        await foreach (var connection in connections)
        {
            var connector = _connectorManager.Create(connection);
            connectors.Add(connector);
        }
    }
}
