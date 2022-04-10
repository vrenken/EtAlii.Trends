// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;

public partial class TimeMap
{
 private void OnConnectorCreating(IDiagramObject diagramObject)
    {
        var connector = (Connector)diagramObject;
        connector.Type = ConnectorSegmentType.Orthogonal;
        connector.TargetDecorator.Shape = DecoratorShape.None;
        connector.Style = new ShapeStyle { StrokeColor = "Black", StrokeWidth = 1 };
        connector.Constraints = 0;
        connector.TargetDecorator = new DecoratorSettings
        {
            Style = new ShapeStyle
            {
                StrokeColor = "#4f4f4f",
                Fill = "#4f4f4f",
                StrokeWidth = 1,
            }
        };
    }
    private void OnPanClick()
    {
        _panItemCssClass = "tb-item-middle tb-item-selected";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _pointerItemCssClass = "tb-item-start";
        _pointerItemCssClass = "tb-item-start";
        _diagramTool = InteractionController.ZoomPan;
        _view = true;
        _center = true;
    }
    private void OnFitToPageClick()
    {
        if (_diagramTool == InteractionController.Default)
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
        _diagram.FitToPage();
    }
    private void OnBringIntoViewClick()
    {
        _panItemCssClass = "tb-item-start";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-middle tb-item-selected";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _pointerItemCssClass = _diagramTool == InteractionController.Default
            ? "tb-item-middle tb-item-selected"
            : "tb-item-start";
        if (_diagram.SelectionSettings.Nodes.Count > 0)
        {
            var node = _diagram.SelectionSettings.Nodes[0];
            var bound = new DiagramRect((node.OffsetX - (node.Width / 2)), node.OffsetY - (node.Height / 2), node.Width, node.Height);
            _diagram.BringIntoView(bound);
        }
    }
    private void OnBringIntoCenterClick()
    {
        _pointerItemCssClass = _diagramTool == InteractionController.Default
            ? "tb-item-middle tb-item-selected"
            : "tb-item-start";
        _panItemCssClass = "tb-item-start";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-middle tb-item-selected";
        _fitCssClass = "tb-item-start";
        if (_diagram.SelectionSettings.Nodes.Count > 0)
        {
            var node = _diagram.SelectionSettings.Nodes[0];
            var bound = new DiagramRect((node.OffsetX - (node.Width / 2)), node.OffsetY - (node.Height / 2), node.Width, node.Height);
            _diagram.BringIntoCenter(bound);
        }
    }
    private void OnPointerClick()
    {
        _diagramTool = InteractionController.SingleSelect | InteractionController.MultipleSelect;
        _panItemCssClass = "tb-item-start";
        _zoomInItemCssClass = "tb-item-start";
        _zoomOutItemCssClass = "tb-item-start";
        _viewCssClass = "tb-item-start";
        _centerCssClass = "tb-item-start";
        _fitCssClass = "tb-item-start";
        _pointerItemCssClass = "tb-item-middle tb-item-selected";
        _view = false;
        _center = false;
    }
    private TreeInfo GetLayoutInfo(IDiagramObject obj, TreeInfo options)
    {
        options.EnableSubTree = true;
        options.Orientation = Orientation.Horizontal;
        return options;
    }

    private void OnZoomInItemClick()
    {
        if (_diagramTool == InteractionController.Default)
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
        _diagram.Zoom(1.2, new DiagramPoint { X = 100, Y = 100 });
    }

    private void OnZoomOutItemClick()
    {
        if (_diagramTool == InteractionController.Default)
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
        _diagram.Zoom(1 / 1.2, new DiagramPoint { X = 100, Y = 100 });
    }

}
