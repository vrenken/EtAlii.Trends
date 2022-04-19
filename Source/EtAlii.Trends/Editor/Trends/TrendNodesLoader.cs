namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class TrendNodesLoader : ITrendNodesLoader
{
    private readonly IQueryDispatcher _queryDispatcher;

    public TrendNodesLoader(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    public async Task Load(DiagramObjectCollection<Node> nodes, Guid diagramId)
    {
        var trends = _queryDispatcher
            .DispatchAsync<Trend>(new GetAllTrendsQuery(diagramId))
            .ConfigureAwait(false);
        await foreach (var trend in trends)
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
            nodes.Add(node);
        }
    }
}
