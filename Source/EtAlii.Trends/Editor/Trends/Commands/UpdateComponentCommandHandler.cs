// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public record UpdateComponentCommand(Component Component) : AsyncCommandWithResult<IUpdateComponentCommandHandler>;

public interface IUpdateComponentCommandHandler : IAsyncCommandHandlerWithResult<UpdateComponentCommand, Component> {}

public class UpdateComponentCommandHandler : IUpdateComponentCommandHandler
{
    public async Task<Component> Handle(UpdateComponentCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Entry(command.Component).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return command.Component;
    }
}


