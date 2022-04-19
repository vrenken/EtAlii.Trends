// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record UpdateConnectionCommand(Connection Connection) : AsyncCommandWithResult<IUpdateConnectionCommandHandler>;

public interface IUpdateConnectionCommandHandler : IAsyncCommandHandlerWithResult<UpdateConnectionCommand, Connection> {}

public class UpdateConnectionCommandHandler : IUpdateConnectionCommandHandler
{
    public async Task<Connection> Handle(UpdateConnectionCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Entry(command.Connection).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return command.Connection;
    }
}


