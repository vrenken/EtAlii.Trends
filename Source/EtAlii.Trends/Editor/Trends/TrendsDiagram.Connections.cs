// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using System.Reflection;
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
                    var command = new AddConnectionCommand
                    (
                        DiagramId: DiagramId,
                        SourceComponentId: Guid.Parse(connector.SourcePortID),
                        TargetComponentId: Guid.Parse(connector.TargetPortID)
                    );
                    var task = _commandDispatcher
                        .DispatchAsync<Connection>(command)
                        .ConfigureAwait(false);

                    var connection = task.GetAwaiter().GetResult();
                    connector.ID = connection.Id.ToString();
                    connector.AdditionalInfo["Connection"] = connection;
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

    private async Task OnConnectionPointChanged(EndPointChangedEventArgs e)
    {
        if (e.Connector.AdditionalInfo.TryGetValue("Connection", out var connectionObject))
        {
            var connection = (Connection)connectionObject;

            var type = typeof(BezierSegment);
            var segment = (BezierSegment)e.Connector.Segments[0];
            var bezierPoint1Property = type.GetProperty("BezierPoint1", BindingFlags.Instance | BindingFlags.NonPublic);
            var bezierPoint1 = (DiagramPoint)bezierPoint1Property!.GetValue(segment)!;
            connection.SourceBezierX = bezierPoint1.X;
            connection.SourceBezierY = bezierPoint1.Y;

            var bezierPoint2Property = type.GetProperty("BezierPoint2", BindingFlags.Instance | BindingFlags.NonPublic);
            var bezierPoint2 = (DiagramPoint)bezierPoint2Property!.GetValue(segment)!;
            connection.TargetBezierX = bezierPoint2.X;
            connection.TargetBezierY = bezierPoint2.Y;

            var command = new UpdateConnectionCommand(connection);
            await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);
        }
    }
}
