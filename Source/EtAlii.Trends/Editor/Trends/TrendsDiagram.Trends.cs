// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using Syncfusion.Blazor.Diagram;

public partial class TrendsDiagram
{

    private void OnTrendsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddTrends(e.NewItems!.Cast<Trend>().ToArray());
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveTrends(e.OldItems!.Cast<Trend>().ToArray());
                break;
        }
    }

    private void RemoveTrends(Trend[] trends)
    {
        foreach (var trend in trends)
        {
            RemoveTrend(trend);
        }
    }

    private void RemoveTrend(Trend trend)
    {
        var node = _nodes.Single(n => n.Data as Trend == trend);
        _nodes.Remove(node);
    }

    private void AddTrend(Trend trend)
    {
        var node = new Node
        {
            Data = trend,
            Height = 100,
            Width = 100,
            ID = trend.Id.ToString(),
            OffsetX = trend.X,
            OffsetY = trend.Y,
            Annotations = new DiagramObjectCollection<ShapeAnnotation>
            {
                new()
                {
                    Content = trend.Name,
                }
            }
        };
        _nodes.Add(node);
    }

    private void AddTrends(Trend[] trends)
    {
        foreach (var trend in trends)
        {
            AddTrend(trend);
        }
    }

    private async Task AddNewTrend(DiagramPoint position)
    {
        var command = new AddTrendCommand
        (
            Trend: diagram => new Trend
            {
                Name = $"New trend {_nodes.Count + 1}",
                X = position.X, Y = position.Y,
                Diagram = diagram,
            },
            DiagramId: DiagramId
        );

        var trend = await _commandDispatcher
            .DispatchAsync<Trend>(command)
            .ConfigureAwait(false);

        AddTrend(trend);

        Trend = trend;
    }
}
