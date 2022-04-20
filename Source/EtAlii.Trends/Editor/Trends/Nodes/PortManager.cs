// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public class PortManager : IPortManager
{
    public PointPort CreatePort(Component component)
    {
        return new PointPort
        {
            ID = component.Id.ToString(),
            Shape = PortShapes.Circle,
            Width = 16,
            Height = 16,
            Visibility = PortVisibility.Visible,
            Offset = new DiagramPoint { X = 0f, Y = 0.5f },
            Style = new ShapeStyle { Fill = "white", StrokeColor = "black" },
            Constraints = PortConstraints.Draw | PortConstraints.InConnect | PortConstraints.OutConnect
        };
    }
}
