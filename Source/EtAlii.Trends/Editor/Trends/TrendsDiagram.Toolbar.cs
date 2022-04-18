// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;

public partial class TrendsDiagram
{
    private string _toolbarHeight = "60px";

    private ToolbarItem? _zoomInItem;
    private string? _zoomInItemCssClass;
    private ToolbarItem? _zoomOutItem;
    private string? _zoomOutItemCssClass;

    private ToolbarItem? _resetItem;
    private string? _resetItemCssClass;

    private ToolbarItem? _panItem;
    private string? _panItemCssClass;
    private ToolbarItem? _pointerItem;
    private string? _pointerItemCssClass;

    private ToolbarItem? _viewItem;
    private string? _viewCssClass;
    private bool _view;

    private ToolbarItem? _centerItem;
    private string? _centerCssClass;
    private bool _center;

    private ToolbarItem? _fitToPageItem;
    private string? _fitCssClass;

    private InteractionController _diagramTool = InteractionController.Default | InteractionController.ContinuousDraw | InteractionController.ZoomPan;
    private SfToolbar? _toolbar;

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

        StateHasChanged();
    }

    private void OnZoomInItemClick()
    {
        if (_diagramTool.HasFlag(InteractionController.Default))
        {
            _pointerItemCssClass = "tb-item-middle tb-item-selected";
            _panItemCssClass= "tb-item-start";
        }
        else
        {
            _pointerItemCssClass = "tb-item-start";
            _panItemCssClass = "tb-item-middle tb-item-selected";
        }
        _zoomInItemCssClass = "tb-item-middle tb-item-selected";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _trendsDiagram.Zoom(1.1f, null); // null causes zoom to happen from the center of the diagram
    }
    private void OnZoomOutItemClick()
    {
        if (_diagramTool.HasFlag(InteractionController.Default))
        {
            _pointerItemCssClass = "tb-item-middle tb-item-selected";
            _panItemCssClass= "tb-item-start";
        }
        else
        {
            _pointerItemCssClass = "tb-item-start";
            _panItemCssClass = "tb-item-middle tb-item-selected";
        }
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-middle tb-item-selected";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _trendsDiagram.Zoom(1 / 1.1f, null); // null causes zoom to happen from the center of the diagram
    }
    //  private void OnPanClick()
    // {
    //     _panItemCssClass = "tb-item-middle tb-item-selected";
    //     _zoomInItemCssClass = "tb-item-start";
    //     _zoomOutItemCssClass = "tb-item-start";
    //     _viewCssClass = "tb-item-start";
    //     _centerCssClass = "tb-item-start";
    //     _fitCssClass = "tb-item-start";
    //     _pointerItemCssClass = "tb-item-start";
    //     _pointerItemCssClass = "tb-item-start";
    //     _diagramTool = InteractionController.ZoomPan | InteractionController.ContinuousDraw;
    //     _view = true;
    //     _center = true;
    // }
    private void OnFitToPageClick()
    {
        if (_diagramTool.HasFlag(InteractionController.Default))
        {
            _pointerItemCssClass = "tb-item-middle tb-item-selected";
            _panItemCssClass= "tb-item-start";
        }
        else
        {
            _pointerItemCssClass = "tb-item-start";
            _panItemCssClass = "tb-item-middle tb-item-selected";
        }
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-middle tb-item-selected";

        _trendsDiagram.FitToPage();
        // double? left = null, top = null, right = null, bottom = null;
        //
        // foreach (var node in _trendsDiagram.Nodes)
        // {
        //     left = !left.HasValue
        //         ? node.OffsetX
        //         : Math.Min(left.Value, node.OffsetX);
        //     top = !top.HasValue
        //         ? node.OffsetY
        //         : Math.Min(top.Value, node.OffsetY);
        //
        //     var nodeRight = node.OffsetX + node.Width;
        //     var nodeBottom = node.OffsetY + node.Height;
        //
        //     right = !right.HasValue
        //         ? nodeRight
        //         : Math.Max(right.Value, nodeRight!.Value);
        //
        //     bottom = !bottom.HasValue
        //         ? nodeBottom
        //         : Math.Max(bottom.Value, nodeBottom!.Value);
        // }
        // var bound = new DiagramRect(left, top, right - left, bottom - top);
        // _trendsDiagram.BringIntoView(bound);
    }
    private void OnBringIntoViewClick()
    {
        _panItemCssClass = "tb-item-start";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-middle tb-item-selected";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _pointerItemCssClass = _diagramTool.HasFlag(InteractionController.Default)
            ? "tb-item-middle tb-item-selected"
            : "tb-item-start";
        if (_trendsDiagram.SelectionSettings.Nodes.Count > 0)
        {
            var node = _trendsDiagram.SelectionSettings.Nodes[0];
            var bound = new DiagramRect((node.OffsetX - (node.Width / 2)), node.OffsetY - (node.Height / 2), node.Width, node.Height);
            _trendsDiagram.BringIntoView(bound);
        }
    }
    private void OnBringIntoCenterClick()
    {
        _pointerItemCssClass = _diagramTool.HasFlag(InteractionController.Default)
            ? "tb-item-middle tb-item-selected"
            : "tb-item-start";
        _panItemCssClass = "tb-item-start";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-middle tb-item-selected";
        _fitCssClass = "tb-item-start";
        if (_trendsDiagram.SelectionSettings.Nodes.Count > 0)
        {
            var node = _trendsDiagram.SelectionSettings.Nodes[0];
            var bound = new DiagramRect((node.OffsetX - (node.Width / 2)), node.OffsetY - (node.Height / 2), node.Width, node.Height);
            _trendsDiagram.BringIntoCenter(bound);
        }
    }
    // private void OnPointerClick()
    // {
    //     _diagramTool = InteractionController.Default | InteractionController.ContinuousDraw;
    //     _panItemCssClass = "tb-item-start";
    //     _zoomInItemCssClass = "tb-item-start";
    //     _zoomOutItemCssClass = "tb-item-start";
    //     _viewCssClass = "tb-item-start";
    //     _centerCssClass = "tb-item-start";
    //     _fitCssClass = "tb-item-start";
    //     _pointerItemCssClass = "tb-item-middle tb-item-selected";
    //     _view = false;
    //     _center = false;
    // }
}
