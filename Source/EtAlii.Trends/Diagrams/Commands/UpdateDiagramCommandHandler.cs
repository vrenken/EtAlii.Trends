// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Diagrams

namespace EtAlii.Trends.Diagrams;

public record UpdateDiagramCommand(Diagram Diagram) : AsyncCommandWithResult<IUpdateDiagramCommandHandler>;

public interface IUpdateDiagramCommandHandler : IAsyncCommandHandlerWithResult<UpdateDiagramCommand, Diagram> {}

public class UpdateDiagramCommandHandler : IUpdateDiagramCommandHandler
{
    public async Task<Diagram> Handle(UpdateDiagramCommand command)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Entry(command.Diagram).State = EntityState.Modified;
        await data
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return command.Diagram;
    }
}


