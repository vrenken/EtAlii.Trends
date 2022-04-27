// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record UpdateConnectionCommand(Func<Trend, Trend, Connection> Connection, Guid SourceTrendId, Guid TargetTrendId) : AsyncCommandWithResult<IUpdateConnectionCommandHandler>;

public interface IUpdateConnectionCommandHandler : IAsyncCommandHandlerWithResult<UpdateConnectionCommand, Connection> {}

public class UpdateConnectionCommandHandler : IUpdateConnectionCommandHandler
{
    public async Task<Connection> Handle(UpdateConnectionCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var source = await data.Trends
            .Where(d => d.Id == command.SourceTrendId)
            .SingleAsync()
            .ConfigureAwait(false);

        var target = await data.Trends
            .Where(d => d.Id == command.TargetTrendId)
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


