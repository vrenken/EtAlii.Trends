// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record RemoveConnectionCommand(Guid ConnectionId) : AsyncCommand<IRemoveConnectionCommandHandler>;

public interface IRemoveConnectionCommandHandler : IAsyncCommandHandler<RemoveConnectionCommand> {}

public class RemoveConnectionCommandHandler : IRemoveConnectionCommandHandler
{
    public async Task Handle(RemoveConnectionCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var layer = await data.Connections
            .AsNoTracking()
            .SingleAsync(l => l.Id == command.ConnectionId)
            .ConfigureAwait(false);

        data.Entry(layer).State = EntityState.Deleted;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}
