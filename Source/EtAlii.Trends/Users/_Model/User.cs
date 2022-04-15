// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using EtAlii.Trends.Diagrams;

public class User : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public IList<Diagram> Diagrams { get; private set; } = new List<Diagram>();
}
