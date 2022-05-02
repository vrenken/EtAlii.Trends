// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using EtAlii.Trends.Diagrams;

public class Connection : Entity
{
    public double SourceBezierAngle { get; set; }
    public double SourceBezierDistance { get; set; }
    public double TargetBezierAngle { get; set; }
    public double TargetBezierDistance { get; set; }

#pragma warning disable CS8618
    public Trend Source { get; set; }

    public Trend Target { get; set; }
    public Diagram Diagram { get; set; }
#pragma warning restore CS8618
}
