// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;
using EtAlii.Trends.Editor.Layers;

public class Trend : DiagramEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }

    public double X { get; set; }
    public double Y { get; set; }

    public double W { get; set; }
    public double H { get; set; }

    public IList<Component> Components { get; init; } = new List<Component>();

    public Layer? Layer { get; set; }
    public bool IsExpanded { get; set; }

    public void SetPositionToTime()
    {
        var beginAsString = Begin.ToString("yyyyMd");
        X = int.Parse(beginAsString);

        var endAsString = End.ToString("yyyyMd");
        H = int.Parse(endAsString) - X;
    }
}
