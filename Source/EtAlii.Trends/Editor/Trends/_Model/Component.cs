// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

public class Component : Entity
{
#pragma warning disable CS8618
    public string Name { get; set; }
    public Trend Trend { get; set; }
#pragma warning restore CS8618
    public DateTime Moment { get; set; }
}
