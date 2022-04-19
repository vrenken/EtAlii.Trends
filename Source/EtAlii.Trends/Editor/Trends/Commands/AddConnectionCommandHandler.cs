// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;

public record AddConnectionCommand(Func<Diagram, Component, Component, Connection> Connection, Guid DiagramId, Guid SourceComponentId, Guid TargetComponentId) : AsyncCommandWithResult<IAddConnectionCommandHandler>;

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

        var source = await data.Components
            .Where(d => d.Id == command.SourceComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var target = await data.Components
            .Where(d => d.Id == command.TargetComponentId)
            .SingleAsync()
            .ConfigureAwait(false);

        var connection = command.Connection(diagram, source, target);

        data.Entry(connection).State = EntityState.Added;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return connection;
    }
}


