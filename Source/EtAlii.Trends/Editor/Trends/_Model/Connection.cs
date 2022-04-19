// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;

public class Connection : Entity
{
#pragma warning disable CS8618
    public Component Source { get; set; }

    public double SourceDirection { get; set; }
    public double SourceLength { get; set; }
    public Component Target { get; set; }
    public double TargetDirection { get; set; }
    public double TargetLength { get; set; }
    public Diagram Diagram { get; set; }
#pragma warning restore CS8618
}
