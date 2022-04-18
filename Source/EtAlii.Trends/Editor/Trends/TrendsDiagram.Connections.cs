// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    private Task OnConnectionChanging(ConnectionChangingEventArgs e)
    {
        return Task.CompletedTask;
    }

    private Task OnConnectionChanged(ConnectionChangedEventArgs e)
    {
        return Task.CompletedTask;
    }
}
