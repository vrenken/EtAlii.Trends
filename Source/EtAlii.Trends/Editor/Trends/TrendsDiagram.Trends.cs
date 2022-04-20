// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using Syncfusion.Blazor.Diagram;

public partial class TrendsDiagram
{
    private async Task AddNewTrend(DiagramPoint position)
    {
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

        var node = _nodeFactory.Create(trend);
        _nodes.Add(node);

        SelectedTrend = trend;
    }
}
