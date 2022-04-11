// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public record Trend(string Id)
{
    public string Name;
    public DateTime Begin;
    public DateTime End;
    public double X;
    public double Y;

    public Layer Layer;
};
