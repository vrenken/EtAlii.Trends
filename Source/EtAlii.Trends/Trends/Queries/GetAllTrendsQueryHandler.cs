// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Trends;

public record GetAllTrendsQuery(Guid DiagramId) : AsyncEnumerableQuery<IGetAllTrendsQueryHandler>;

public interface IGetAllTrendsQueryHandler : IAsyncEnumerableQueryHandler<GetAllTrendsQuery, Trend> {}

public class GetAllTrendsQueryHandler : IGetAllTrendsQueryHandler
{
    public async IAsyncEnumerable<Trend> Handle(GetAllTrendsQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var items = data.Trends
            .AsNoTracking()
            .Include(t => t.Diagram)
            .Where(t => t.Diagram.Id == query.DiagramId)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item;
        }
    }
}


