// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    private async Task AddNewTrend(DiagramPoint position)
    {
        _log.Verbose("Method called {MethodName}", nameof(AddNewTrend));

        var command = new AddTrendCommand
        (
            Trend: diagram => new Trend
            {
                Name = $"New trend {_nodes.Count + 1}",
                X = position.X, Y = position.Y,
                Diagram = diagram,
                Begin = DateTime.Now,
                End = DateTime.Now.AddMonths(1)
            },
            DiagramId: DiagramId
        );

        var trend = await _commandDispatcher
            .DispatchAsync<Trend>(command)
            .ConfigureAwait(false);

        var node = _nodeManager.Create(trend);
        _nodes.Add(node);

        SelectedTrend = trend;
    }


    private async Task OnDeleteItems()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnDeleteItems));

        var nodesToRemove = _selectedDiagramObjects
            .OfType<Node>()
            .ToArray();

        var trendsToDelete = nodesToRemove
            .Select(n => n.Data)
            .Where(t => t != null)
            .Cast<Trend>()
            .ToArray();

        var connectorsToRemove = _selectedDiagramObjects
            .OfType<Connector>()
            .ToArray();

        var connectionsToDelete = connectorsToRemove
            .Select(n =>
            {
                if(n.AdditionalInfo.TryGetValue("Connection", out var connection))
                {
                    return connection;
                }
                return null;
            })
            .Where(c => c != null)
            .Cast<Connection>();

        foreach (var trend in trendsToDelete)
        {
            var command = new RemoveTrendCommand(trend.Id);
            await _commandDispatcher
                .DispatchAsync(command)
                .ConfigureAwait(false);
        }

        foreach (var connection in connectionsToDelete)
        {
            var command = new RemoveConnectionCommand(connection.Id);
            await _commandDispatcher
                .DispatchAsync(command)
                .ConfigureAwait(false);
        }

        foreach (var connectorToRemove in connectorsToRemove)
        {
            _connectors.Remove(connectorToRemove);
        }

        foreach (var nodeToRemove in nodesToRemove)
        {
            _nodes.Remove(nodeToRemove);
        }
    }
}
