namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class TrendNodesLoader : ITrendNodesLoader
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly INodeFactory _nodeFactory;

    public TrendNodesLoader(IQueryDispatcher queryDispatcher, INodeFactory nodeFactory)
    {
        _queryDispatcher = queryDispatcher;
        _nodeFactory = nodeFactory;
    }

    public async Task Load(DiagramObjectCollection<Node> nodes, Guid diagramId)
    {
        var trends = _queryDispatcher
            .DispatchAsync<Trend>(new GetAllTrendsQuery(diagramId))
            .ConfigureAwait(false);

        await foreach (var trend in trends)
        {
            var node = _nodeFactory.Create(trend);
            nodes.Add(node);
        }
    }
}
