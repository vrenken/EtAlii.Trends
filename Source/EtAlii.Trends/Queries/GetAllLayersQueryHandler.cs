// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Linq;
using Microsoft.EntityFrameworkCore;

public record GetAllLayersQuery(Guid DiagramId) : AsyncEnumerableQuery<IGetAllLayersQueryHandler>;

public interface IGetAllLayersQueryHandler : IAsyncEnumerableQueryHandler<GetAllLayersQuery, Layer> {}

public class GetAllLayersQueryHandler : IGetAllLayersQueryHandler
{
    public async IAsyncEnumerable<Layer> Handle(GetAllLayersQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var items = data.Layers
            .AsNoTracking()
            .Include(l => l.Diagram)
            .Where(l => l.Diagram.Id == query.DiagramId)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item;
        }
    }
}


