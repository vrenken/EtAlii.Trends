// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;

public partial class TrendsDiagram
{
    private string _toolbarHeight = "60px";

    private ToolbarItem _zoomInItem;
    private ToolbarItem _zoomOutItem;

    private ToolbarItem _resetItem;

    private ToolbarItem _panItem;
    private InteractionController _panZoomController = InteractionController.ZoomPan;

    private ToolbarItem _editTrendItem;
    private InteractionController _editTrendController = InteractionController.MultipleSelect | InteractionController.SingleSelect;

    private ToolbarItem _fitToPageItem;

    private InteractionController _diagramTool = InteractionController.ZoomPan;
    private SfToolbar _toolbar;

    private async Task OnResetItemClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnResetItemClick));

        var diagram = await _queryDispatcher
            .DispatchAsync<Diagram>(new GetDiagramQuery(DiagramId))
            .ConfigureAwait(false);

        diagram.HorizontalOffset = Diagram.StartOffset;
        diagram.VerticalOffset = Diagram.StartOffset;
        diagram.DiagramZoom = Diagram.StartZoom;

        await _commandDispatcher.DispatchAsync<Diagram>(new UpdateDiagramCommand(diagram))
            .ConfigureAwait(false);

        _currentZoom = diagram.DiagramZoom;
        _horizontalOffset = diagram.HorizontalOffset;
        _verticalOffset = diagram.VerticalOffset;
    }

    private void OnZoomInItemClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnZoomInItemClick));

        _trendsDiagram.Zoom(1.1f, null); // null causes zoom to happen from the center of the diagram
    }

    private void OnZoomOutItemClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnZoomOutItemClick));

        _trendsDiagram.Zoom(1 / 1.1f, null); // null causes zoom to happen from the center of the diagram
    }

    private void OnFitToPageClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnFitToPageClick));

        _trendsDiagram.FitToPage();
    }

    private void OnBringIntoCenterClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnBringIntoCenterClick));

        _viewManager.OnBringIntoCenter(_trendsDiagram, _selectedDiagramObjects.AsReadOnly());
    }

    private void OnBringIntoViewClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnBringIntoViewClick));

        _viewManager.OnBringIntoView(_trendsDiagram, _selectedDiagramObjects.AsReadOnly());
    }

    private Task OnDeleteItemsClicked()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnDeleteItemsClicked));

        return OnDeleteItems();
    }

    private void OnPanClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnPanClick));

        _diagramTool = _panZoomController;
    }

    private void OnEditTrendClick()
    {
        _log.Verbose("Method called {MethodName}", nameof(OnEditTrendClick));

        _diagramTool = _editTrendController;
    }
}
