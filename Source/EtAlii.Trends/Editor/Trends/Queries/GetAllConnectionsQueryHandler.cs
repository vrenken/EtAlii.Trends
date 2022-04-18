// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record GetAllConnectionsQuery(Guid DiagramId) : AsyncEnumerableQuery<IGetAllConnectionsQueryHandler>;

public interface IGetAllConnectionsQueryHandler : IAsyncEnumerableQueryHandler<GetAllConnectionsQuery, Connection> {}

public class GetAllConnectionsQueryHandler : IGetAllConnectionsQueryHandler
{
    public async IAsyncEnumerable<Connection> Handle(GetAllConnectionsQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var items = data.Connections
            .AsNoTracking()
            .Include(connection => connection.From)
            .ThenInclude(component => component.Trend)
            .Include(connection => connection.To)
            .ThenInclude(component => component.Trend)
            .Include(connection => connection.Diagram)
            .Where(connection => connection.Diagram.Id == query.DiagramId)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item;
        }
    }
}


