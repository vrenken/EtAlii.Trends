// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Linq;
using Microsoft.EntityFrameworkCore;

public record AddTrendsCommand(Func<Diagram, Trend> Trend, Guid DiagramId) : AsyncCommandWithResult<IAddTrendsCommandHandler>;

public interface IAddTrendsCommandHandler : IAsyncCommandHandlerWithResult<AddTrendsCommand, Trend> {}

public class AddTrendsCommandHandler : IAddTrendsCommandHandler
{
    public async Task<Trend> Handle(AddTrendsCommand command)
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


