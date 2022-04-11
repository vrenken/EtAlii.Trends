// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public record Trend(Guid Id)
{
    public string Name = string.Empty;
    public DateTime Begin;
    public DateTime End;
    public double X;
    public double Y;

    public Component[] Components = Array.Empty<Component>();

    public Layer Layer;
    public bool IsExpanded;

    public Diagram Diagram;
}
