// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public interface IComponentConnectionLoader
{
    Task Load(DiagramObjectCollection<Connector> nodes, Guid diagramId);
}
