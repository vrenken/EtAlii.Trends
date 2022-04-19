// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ComponentConnectionLoader : IComponentConnectionLoader
{
    private readonly IQueryDispatcher _queryDispatcher;

    public ComponentConnectionLoader(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    public async Task Load(DiagramObjectCollection<Connector> connectors, Guid diagramId)
    {
        var connections = _queryDispatcher
            .DispatchAsync<Connection>(new GetAllConnectionsQuery(diagramId))
            .ConfigureAwait(false);
        await foreach (var connection in connections)
        {
            var connector = new Connector
            {
                CanAutoLayout = true,
                ID = connection.Id.ToString(),
                SourceID = connection.From.Trend.Id.ToString(),
                SourcePortID = connection.From.Id.ToString(),
                TargetID = connection.To.Trend.Id.ToString(),
                TargetPortID = connection.To.Id.ToString()
            };
            connectors.Add(connector);
        }
    }
}
