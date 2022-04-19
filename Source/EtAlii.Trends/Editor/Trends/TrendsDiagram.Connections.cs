// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
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

            var sourceHigherThanTarget = e.Connector.SourcePoint.Y < e.Connector.TargetPoint.Y;

            var delta = sourceHigherThanTarget
                ? e.Connector.TargetPoint.Y - e.Connector.SourcePoint.Y
                : e.Connector.SourcePoint.Y - e.Connector.TargetPoint.Y;

            connection.SourceBezierAngle = sourceHigherThanTarget ? +90 : -90;
            connection.SourceBezierDistance = delta / 3f * 2f;
            connection.TargetBezierAngle = sourceHigherThanTarget ? -90 : +90;
            connection.TargetBezierDistance = delta / 3f * 2f;

            // The below angle and distances do not change. Feels weird.
            // var segment = (BezierSegment)e.Connector.Segments[0];
            // connection.SourceBezierAngle = segment.Vector1.Angle;
            // connection.SourceBezierDistance = segment.Vector1.Distance;
            // connection.TargetBezierAngle = segment.Vector2.Angle;
            // connection.TargetBezierDistance = segment.Vector2.Distance;

            var command = new UpdateConnectionCommand(connection);
            await _commandDispatcher
                .DispatchAsync<Connection>(command)
                .ConfigureAwait(false);
        }
    }
}
