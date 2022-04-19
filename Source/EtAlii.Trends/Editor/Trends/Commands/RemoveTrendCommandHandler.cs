// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record RemoveTrendCommand(Guid TrendId) : AsyncCommand<IRemoveTrendCommandHandler>;

public interface IRemoveTrendCommandHandler : IAsyncCommandHandler<RemoveTrendCommand> {}

public class RemoveTrendCommandHandler : IRemoveTrendCommandHandler
{
    public async Task Handle(RemoveTrendCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var trend = await data.Trends
            .AsNoTracking()
            .SingleAsync(l => l.Id == command.TrendId)
            .ConfigureAwait(false);

        data.Entry(trend).State = EntityState.Deleted;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}
