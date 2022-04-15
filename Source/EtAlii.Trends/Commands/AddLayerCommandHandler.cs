// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Linq;
using Microsoft.EntityFrameworkCore;

public record AddLayerCommand(Func<Diagram, Layer, Layer> Layer, Guid DiagramId, Guid? ParentLayerId) : AsyncCommandWithResult<IAddLayerCommandHandler>;

public interface IAddLayerCommandHandler : IAsyncCommandHandlerWithResult<AddLayerCommand, Layer> {}

public class AddLayerCommandHandler : IAddLayerCommandHandler
{
    public async Task<Layer> Handle(AddLayerCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        var diagram = await data.Diagrams
            .Where(d => d.Id == command.DiagramId)
            .SingleAsync()
            .ConfigureAwait(false);

        var parentLayer = command.ParentLayerId == null
            ? null!
            : await data.Layers
                .Where(l => l.Id == command.ParentLayerId)
                .SingleAsync()
                .ConfigureAwait(false);

        var layer = command.Layer(diagram, parentLayer);

        data.Entry(layer).State = EntityState.Added;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return layer;
    }
}


