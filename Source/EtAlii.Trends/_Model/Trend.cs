// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public class Trend : Entity
{
    public string Name { get; set; } = string.Empty;
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public Component[] Components = Array.Empty<Component>();

    public Layer? Layer { get; set; }
    public bool IsExpanded { get; set; }

    public Diagram? Diagram { get; set; }
}
