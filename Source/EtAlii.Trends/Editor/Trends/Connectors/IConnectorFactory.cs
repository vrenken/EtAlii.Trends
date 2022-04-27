// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public interface IConnectorFactory
{
    Connector Create(Connection connection);
    Connector CreateBlank();

    void ApplyStyle(Connector connector);

    void Recalculate(Connector connector);
}
