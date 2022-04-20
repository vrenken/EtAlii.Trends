namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class TrendNodesLoader : ITrendNodesLoader
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly INodeManager _nodeManager;

    public TrendNodesLoader(IQueryDispatcher queryDispatcher, INodeManager nodeManager)
    {
        _queryDispatcher = queryDispatcher;
        _nodeManager = nodeManager;
    }

    public async Task Load(DiagramObjectCollection<Node> nodes, Guid diagramId)
    {
        var trends = _queryDispatcher
            .DispatchAsync<Trend>(new GetAllTrendsQuery(diagramId))
            .ConfigureAwait(false);

        await foreach (var trend in trends)
        {
            var node = _nodeManager.Create(trend);
            nodes.Add(node);
        }
    }
}
