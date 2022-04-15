// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Diagrams;

public record GetAllDiagramsForUserQuery(Guid UserId) : AsyncEnumerableQuery<IGetAllDiagramsForUserQueryHandler>;

public interface IGetAllDiagramsForUserQueryHandler : IAsyncEnumerableQueryHandler<GetAllDiagramsForUserQuery, Diagram> {}

public class GetAllDiagramsForUserQueryHandler : IGetAllDiagramsForUserQueryHandler
{
    public async IAsyncEnumerable<Diagram> Handle(GetAllDiagramsForUserQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var items = data.Diagrams
            .AsNoTracking()
            .Include(d => d.User)
            .Where(d => d.User.Id == query.UserId)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item;
        }
    }
}


