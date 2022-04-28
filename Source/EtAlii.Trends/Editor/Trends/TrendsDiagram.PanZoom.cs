// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public partial class TrendsDiagram
{
    //Sets the line intervals for the gridlines.
    private readonly double[] _lineInterval = { 1, 9, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75 };
    private readonly SnapConstraints _snapConstraints = SnapConstraints.ShowLines | SnapConstraints.SnapToObject;

    private readonly ThrottledInvocation _persistPanAndZoom;

    private async Task InitializePositionAndZoom()
    {
        _log.Verbose("Method called {MethodName}", nameof(InitializePositionAndZoom));

        var diagram = await _queryDispatcher
            .DispatchAsync<Diagram>(new GetDiagramQuery(DiagramId))
            .ConfigureAwait(false);

#pragma warning disable BL0005
        _trendsDiagram.ScrollSettings.CurrentZoom = diagram.DiagramZoom;
        _trendsDiagram.ScrollSettings.HorizontalOffset = diagram.HorizontalOffset;
        _trendsDiagram.ScrollSettings.VerticalOffset = diagram.VerticalOffset;
#pragma warning restore BL0005
    }

    private async Task PersistPanAndZoom()
    {
        _log.Verbose("Method called {MethodName}", nameof(PersistPanAndZoom));

        var diagram = await _queryDispatcher
            .DispatchAsync<Diagram>(new GetDiagramQuery(DiagramId))
            .ConfigureAwait(false);

        diagram.DiagramZoom = _trendsDiagram.ScrollSettings.CurrentZoom;
        diagram.HorizontalOffset = _trendsDiagram.ScrollSettings.HorizontalOffset;
        diagram.VerticalOffset = _trendsDiagram.ScrollSettings.VerticalOffset;

        await _commandDispatcher.DispatchAsync<Diagram>(new UpdateDiagramCommand(diagram))
            .ConfigureAwait(false);
    }

    private void OnCurrentZoomChanged(double currentZoom) => _persistPanAndZoom.Raise();

    private void OnHorizontalOffsetChanged(double horizontalOffset) => _persistPanAndZoom.Raise();

    private void OnVerticalOffsetChanged(double verticalOffset) => _persistPanAndZoom.Raise();
}
