// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;

public partial class TrendsDiagram
{
    private string _toolbarHeight = "60px";

    private ToolbarItem _zoomInItem;
    private string _zoomInCssClass;
    private ToolbarItem _zoomOutItem;
    private string _zoomOutCssClass;

    private ToolbarItem _resetItem;
    private string _resetItemCssClass;

    private ToolbarItem _panItem;
    private string _panZoomCssClass;
    private InteractionController _panZoomController = InteractionController.ZoomPan;

    private ToolbarItem _editTrendItem;
    private string _editTrendCssClass;
    private InteractionController _editTrendController = InteractionController.MultipleSelect | InteractionController.SingleSelect;

    private ToolbarItem _editConnectionItem;
    private string _editConnectionCssClass;
    private InteractionController _editConnectionController = InteractionController.ContinuousDraw;

    private ToolbarItem _fitToPageItem;
    private string _fitCssClass;

    private InteractionController _diagramTool = InteractionController.Default | InteractionController.ZoomPan;
    private SfToolbar _toolbar;

    private async Task OnResetItemClick()
    {
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

        UpdateButtons();
    }

    private void OnZoomInItemClick()
    {
        _trendsDiagram.Zoom(1.1f, null); // null causes zoom to happen from the center of the diagram
        UpdateButtons();
    }

    private void OnZoomOutItemClick()
    {
        _trendsDiagram.Zoom(1 / 1.1f, null); // null causes zoom to happen from the center of the diagram
        UpdateButtons();
    }

    private void OnFitToPageClick()
    {
        _trendsDiagram.FitToPage();
        UpdateButtons();
    }

    private void OnBringIntoCenterClick() => _viewManager.OnBringIntoCenter(_trendsDiagram, _selectedDiagramObjects.AsReadOnly());

    private void OnBringIntoViewClick() => _viewManager.OnBringIntoView(_trendsDiagram, _selectedDiagramObjects.AsReadOnly());

    private Task OnDeleteItemsClicked() => OnDeleteItems();

    private void OnPanClick()
    {
        _diagramTool = _panZoomController;
        _drawingObjectFactory = null;
        UpdateButtons();
    }

    private void OnEditTrendClick()
    {
        _diagramTool = _editTrendController;
        _drawingObjectFactory = null;
        UpdateButtons();
    }

    private void OnEditConnectionClick()
    {
        _diagramTool = _editConnectionController;
        _drawingObjectFactory = () => _connectorFactory.CreateBlank();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        _zoomInCssClass = "tb-item-start";
        _zoomOutCssClass = "tb-item-start";

        _panZoomCssClass = _diagramTool.HasFlag(_panZoomController) ? "tb-item-middle tb-item-selected" : "tb-item-start";
        _editTrendCssClass = _diagramTool.HasFlag(_editTrendController) ? "tb-item-middle tb-item-selected" : "tb-item-start";
        _editConnectionCssClass = _diagramTool.HasFlag(_editConnectionController) ? "tb-item-middle tb-item-selected" : "tb-item-start";

        _resetItemCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
    }
}
