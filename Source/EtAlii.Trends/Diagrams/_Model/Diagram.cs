// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Diagrams;

using EtAlii.Trends.Editor.Layers;
using EtAlii.Trends.Editor.Trends;

#pragma warning disable CA1724
public class Diagram : Entity
{
    public string Name { get; set; } = string.Empty;

    public float DiagramWidth { get; set; }
    public float PropertyGridHeight { get; set; }
    public double DiagramZoom { get; set; }

    public double DiagramTimePosition { get; set; }
    public double DiagramVerticalPosition { get; set; }

    public IList<Trend> Trends { get; private set; } = new List<Trend>();
    public IList<Layer> Layers { get; private set; } = new List<Layer>();

#pragma warning disable CS8618
    public User User { get; init; }
#pragma warning restore CS8618

    public static void ResetPanZoom(Diagram diagram)
    {
        var nowAsString = DateTime.Now.ToString("yyyyMd");
        diagram.DiagramTimePosition = int.Parse(nowAsString);
        diagram.DiagramVerticalPosition = 2000;
        diagram.DiagramZoom = 1d;
    }
}
