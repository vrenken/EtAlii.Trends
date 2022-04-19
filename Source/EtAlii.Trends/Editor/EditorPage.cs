// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor;

using EtAlii.Trends.Diagrams;
using EtAlii.Trends.Editor.Trends;
using Syncfusion.Blazor.Layouts;

public partial class EditorPage
{
    private Guid _diagramId;

#pragma warning disable CS8618
    private TrendsDiagram _trendsDiagram;
    private TrendPropertyGrid _trendPropertyGrid;
#pragma warning restore CS8618

    private Trend? _selectedTrend;
    protected override async Task OnInitializedAsync()
    {
        var user = await _queryDispatcher.DispatchAsync<User>(new GetUserQuery(Guid.Empty)).ConfigureAwait(false);
        // var diagrams = _queryDispatcher
        //     .DispatchAsync<Diagram>(new GetAllDiagramsForUserQuery(user.Id))
        //     .ConfigureAwait(false);
        var diagram = await _queryDispatcher.DispatchAsync<Diagram>(new GetDiagramQuery(Guid.Empty)).ConfigureAwait(false);

        _diagramId = diagram.Id;
    }

    private void OnSplitterResized(ResizingEventArgs arg) => _trendsDiagram.OnSplitterResized(arg);

    private void OnSelectedTrendChanged(Trend? trend)
    {
        _selectedTrend = trend;
    }
}
