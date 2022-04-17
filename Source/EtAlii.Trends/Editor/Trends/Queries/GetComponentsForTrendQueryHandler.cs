// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record GetComponentsForTrendQuery(Guid TrendId) : AsyncEnumerableQuery<IGetComponentsForTrendQueryHandler>;

public interface IGetComponentsForTrendQueryHandler : IAsyncEnumerableQueryHandler<GetComponentsForTrendQuery, Component> {}

public class GetComponentsForTrendQueryHandler : IGetComponentsForTrendQueryHandler
{
    public async IAsyncEnumerable<Component> Handle(GetComponentsForTrendQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var items = data.Components
            .AsNoTracking()
            .Include(c => c.Trend)
            .Where(c => c.Trend.Id == query.TrendId)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item;
        }
    }
}


