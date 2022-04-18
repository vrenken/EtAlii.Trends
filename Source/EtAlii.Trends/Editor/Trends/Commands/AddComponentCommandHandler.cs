// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record AddComponentCommand(Func<Trend, Component> Component, Guid TrendId) : AsyncCommandWithResult<IAddComponentCommandHandler>;

public interface IAddComponentCommandHandler : IAsyncCommandHandlerWithResult<AddComponentCommand, Component> {}

public class AddComponentCommandHandler : IAddComponentCommandHandler
{
    public async Task<Component> Handle(AddComponentCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var trend = await data.Trends
            .Where(d => d.Id == command.TrendId)
            .SingleAsync()
            .ConfigureAwait(false);

        var component = command.Component(trend);

        data.Entry(component).State = EntityState.Added;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return component;
    }
}
