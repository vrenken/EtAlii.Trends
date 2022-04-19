// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    private void OnConnectorsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (Connector connector in e.NewItems!)
                {
                    // if (!string.IsNullOrWhiteSpace(connector.SourcePortID) && !string.IsNullOrWhiteSpace(connector.TargetPortID))
                    // {
                        var command = new AddConnectionCommand
                        (
                            DiagramId: DiagramId,
                            FromComponentId: Guid.Parse(connector.SourcePortID),
                            ToComponentId: Guid.Parse(connector.TargetPortID)
                        );
                        var task = _commandDispatcher
                            .DispatchAsync<Connection>(command)
                            .ConfigureAwait(false);

                        var connection = task.GetAwaiter().GetResult();
                        connector.ID = connection.Id.ToString();
                    // }
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
        }
    }

    private Task OnConnectionChanging(ConnectionChangingEventArgs e)
    {
        switch (e.ConnectorAction)
        {
            case Actions.ConnectorSourceEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                {
                    // No port, let's cancel.
                    e.Cancel = true;
                }
                break;
            case Actions.ConnectorTargetEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID))
                {
                    // No port, let's cancel.
                    e.Cancel = true;

                    if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID) ||
                        string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                    {
                        _connectors.Remove(e.Connector);
                    }
                }
                else
                {

                }
                break;
        }

        return Task.CompletedTask;
    }

    private Task OnConnectionChanged(ConnectionChangedEventArgs e)
    {
        switch (e.ConnectorAction)
        {
            case Actions.ConnectorSourceEnd:
            case Actions.ConnectorTargetEnd:
                if (string.IsNullOrWhiteSpace(e.NewValue.TargetPortID) ||
                    string.IsNullOrWhiteSpace(e.NewValue.SourcePortID))
                {
                    _connectors.Remove(e.Connector);
                }
                break;
        }
        return Task.CompletedTask;
    }
}
