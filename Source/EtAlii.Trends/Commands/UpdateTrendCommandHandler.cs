// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;

public record UpdateTrendCommand(Trend Trend) : AsyncCommandWithResult<IUpdateTrendCommandHandler>;

public interface IUpdateTrendCommandHandler : IAsyncCommandHandlerWithResult<UpdateTrendCommand, Trend> {}

public class UpdateTrendCommandHandler : IUpdateTrendCommandHandler
{
    public async Task<Trend> Handle(UpdateTrendCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Entry(command.Trend).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return command.Trend;
    }
}


