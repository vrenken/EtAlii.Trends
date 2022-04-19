// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ComponentConnectionLoader : IComponentConnectionLoader
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IConnectorFactory _connectorFactory;

    public ComponentConnectionLoader(IQueryDispatcher queryDispatcher, IConnectorFactory connectorFactory)
    {
        _queryDispatcher = queryDispatcher;
        _connectorFactory = connectorFactory;
    }

    public async Task Load(DiagramObjectCollection<Connector> connectors, DiagramObjectCollection<Node> nodes, Guid diagramId)
    {
        var connections = _queryDispatcher
            .DispatchAsync<Connection>(new GetAllConnectionsQuery(diagramId))
            .ConfigureAwait(false);

        await foreach (var connection in connections)
        {
            var connector = _connectorFactory.Create(connection);
            connectors.Add(connector);
        }
    }
}
