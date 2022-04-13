// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;

public record GetDiagramQuery(Guid DiagramId) : AsyncQuery<IGetDiagramQueryHandler>;

public interface IGetDiagramQueryHandler : IAsyncQueryHandler<GetDiagramQuery, Diagram> {}

public class GetDiagramQueryHandler : IGetDiagramQueryHandler
{
    public async Task<Diagram> Handle(GetDiagramQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        if (query.DiagramId == Guid.Empty)
        {
            // TODO: Temp patch. Should be removed.
            return await data.Diagrams
                .AsNoTracking()
                .SingleAsync()
                .ConfigureAwait(false);
        }
        return await data.Diagrams
            .AsNoTracking()
            .Where(d => d.Id == query.DiagramId)
            .SingleAsync()
            .ConfigureAwait(false);
    }
}


