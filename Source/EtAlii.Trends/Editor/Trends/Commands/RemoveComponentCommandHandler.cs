// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record RemoveComponentCommand(Guid ComponentId) : AsyncCommand<IRemoveComponentCommandHandler>;

public interface IRemoveComponentCommandHandler : IAsyncCommandHandler<RemoveComponentCommand> {}

public class RemoveComponentCommandHandler : IRemoveComponentCommandHandler
{
    public async Task Handle(RemoveComponentCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var component = await data.Components
            .AsNoTracking()
            .SingleAsync(l => l.Id == command.ComponentId)
            .ConfigureAwait(false);

        data.Entry(component).State = EntityState.Deleted;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}
