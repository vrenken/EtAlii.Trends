// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record UpdateConnectionCommand(Func<Component, Component, Connection> Connection, Guid SourceComponentId, Guid TargetComponentId) : AsyncCommandWithResult<IUpdateConnectionCommandHandler>;

public interface IUpdateConnectionCommandHandler : IAsyncCommandHandlerWithResult<UpdateConnectionCommand, Connection> {}

public class UpdateConnectionCommandHandler : IUpdateConnectionCommandHandler
{
    public async Task<Connection> Handle(UpdateConnectionCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var source = await data.Components
            .Where(d => d.Id == command.SourceComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var target = await data.Components
            .Where(d => d.Id == command.TargetComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var connection = command.Connection(source, target);

        data.Entry(connection).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return connection;
    }
}


