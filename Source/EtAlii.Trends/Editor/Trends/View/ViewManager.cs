// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class ViewManager : IViewManager
{
    public void OnBringIntoCenter(SfDiagramComponent diagramComponent, IReadOnlyList<IDiagramObject> selectedDiagramObjects)
    {
        double? left = null, top = null, right = null, bottom = null;

        foreach (var node in selectedDiagramObjects.OfType<Node>())
        {
            left = !left.HasValue
                ? node.OffsetX
                : Math.Min(left.Value, node.OffsetX);
            top = !top.HasValue
                ? node.OffsetY
                : Math.Min(top.Value, node.OffsetY);

            var nodeRight = node.OffsetX + node.Width;
            var nodeBottom = node.OffsetY + node.Height;

            right = !right.HasValue
                ? nodeRight
                : Math.Max(right.Value, nodeRight!.Value);

            bottom = !bottom.HasValue
                ? nodeBottom
                : Math.Max(bottom.Value, nodeBottom!.Value);

        }
        var bound = new DiagramRect(left, top, right - left, bottom - top);
        diagramComponent.BringIntoCenter(bound);
    }

    public void OnBringIntoView(SfDiagramComponent diagramComponent, IReadOnlyList<IDiagramObject> selectedDiagramObjects)
    {
        double? left = null, top = null, right = null, bottom = null;

        foreach (var node in selectedDiagramObjects.OfType<Node>())
        {
            left = !left.HasValue
                ? node.OffsetX
                : Math.Min(left.Value, node.OffsetX);
            top = !top.HasValue
                ? node.OffsetY
                : Math.Min(top.Value, node.OffsetY);

            var nodeRight = node.OffsetX + node.Width;
            var nodeBottom = node.OffsetY + node.Height;

            right = !right.HasValue
                ? nodeRight
                : Math.Max(right.Value, nodeRight!.Value);

            bottom = !bottom.HasValue
                ? nodeBottom
                : Math.Max(bottom.Value, nodeBottom!.Value);

        }
        var bound = new DiagramRect(left, top, right - left, bottom - top);
        diagramComponent.BringIntoView(bound);
    }
}
