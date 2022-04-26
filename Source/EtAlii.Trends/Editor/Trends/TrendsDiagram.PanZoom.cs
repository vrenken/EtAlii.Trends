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
    private double _currentZoom;
    private double _horizontalOffset;
    private double _verticalOffset;

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

        _currentZoom = diagram.DiagramZoom;
        _horizontalOffset = diagram.HorizontalOffset;
        _verticalOffset = diagram.VerticalOffset;

        StateHasChanged();
    }

    private void OnCurrentZoomChanged(double currentZoom)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnCurrentZoomChanged));

        _currentZoom = currentZoom;
        _persistPanAndZoom.Raise();
    }

    private void OnHorizontalOffsetChanged(double horizontalOffset)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnHorizontalOffsetChanged));

        _horizontalOffset = horizontalOffset;
        _persistPanAndZoom.Raise();
    }

    private void OnVerticalOffsetChanged(double verticalOffset)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnVerticalOffsetChanged));

        _verticalOffset = verticalOffset;
        _persistPanAndZoom.Raise();
    }
}
