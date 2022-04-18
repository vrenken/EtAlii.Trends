// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record AddConnectionCommand(Guid DiagramId, Guid FromComponentId, Guid ToComponentId) : AsyncCommandWithResult<IAddConnectionCommandHandler>;

public interface IAddConnectionCommandHandler : IAsyncCommandHandlerWithResult<AddConnectionCommand, Connection> {}

public class AddConnectionCommandHandler : IAddConnectionCommandHandler
{
    public async Task<Connection> Handle(AddConnectionCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var diagram = await data.Diagrams
            .Where(d => d.Id == command.DiagramId)
            .SingleAsync()
            .ConfigureAwait(false);

        var from = await data.Components
            .Where(d => d.Id == command.FromComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var to = await data.Components
            .Where(d => d.Id == command.ToComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var connection = new Connection
        {
            Diagram = diagram,
            From = from,
            To = to
        };

        data.Entry(connection).State = EntityState.Added;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return connection;
    }
}


