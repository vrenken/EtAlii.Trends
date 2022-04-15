// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;

public record AddTrendCommand(Func<Diagram, Trend> Trend, Guid DiagramId) : AsyncCommandWithResult<IAddTrendCommandHandler>;

public interface IAddTrendCommandHandler : IAsyncCommandHandlerWithResult<AddTrendCommand, Trend> {}

public class AddTrendCommandHandler : IAddTrendCommandHandler
{
    public async Task<Trend> Handle(AddTrendCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var diagram = await data.Diagrams
            .Where(d => d.Id == command.DiagramId)
            .SingleAsync()
            .ConfigureAwait(false);

        var trend = command.Trend(diagram);//.Diagram = diagram;

        data.Entry(trend).State = EntityState.Added;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return trend;
    }
}


