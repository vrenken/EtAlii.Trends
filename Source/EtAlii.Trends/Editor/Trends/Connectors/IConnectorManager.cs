// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;

public interface IConnectorManager
{
    Connector Create(Connection connection);

    void ApplyStyle(Connector connector);

    Task Recalculate(Connector connector);

    Task Recalculate(Node node, DiagramObjectCollection<Connector> connectors);

    void UpdateConnectionFromConnector(Connector connector, Connection connection);

    void UpdateConnectorFromConnection(Connection connection, Connector connector);
}
