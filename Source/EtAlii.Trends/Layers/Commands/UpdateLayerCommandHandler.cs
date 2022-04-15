// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Layers;

public record UpdateLayerCommand(Layer Layer) : AsyncCommandWithResult<IUpdateLayerCommandHandler>;

public interface IUpdateLayerCommandHandler : IAsyncCommandHandlerWithResult<UpdateLayerCommand, Layer> {}

public class UpdateLayerCommandHandler : IUpdateLayerCommandHandler
{
    public async Task<Layer> Handle(UpdateLayerCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Entry(command.Layer).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return command.Layer;
    }
}


