// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

#pragma warning disable CA1724
public class Diagram : Entity
{
    public string Name { get; set; } = string.Empty;

    public IList<Trend> Trends { get; private set; } = new List<Trend>();
    public IList<Layer> Layers { get; private set; } = new List<Layer>();

    public User User { get; init; }

}
