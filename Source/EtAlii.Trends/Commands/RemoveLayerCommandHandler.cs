// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;

public record RemoveLayerCommand(Guid LayerId) : AsyncCommand<IRemoveLayerCommandHandler>;

public interface IRemoveLayerCommandHandler : IAsyncCommandHandler<RemoveLayerCommand> {}

public class RemoveLayerCommandHandler : IRemoveLayerCommandHandler
{
    public async Task Handle(RemoveLayerCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var layer = await data.Layers
            .AsNoTracking()
            .SingleAsync(l => l.Id == command.LayerId)
            .ConfigureAwait(false);

        data.Entry(layer).State = EntityState.Deleted;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}


