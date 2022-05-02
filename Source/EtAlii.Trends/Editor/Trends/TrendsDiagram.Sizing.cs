// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    private bool _isLoaded;
    private string _diagramHeight = "900px";
    private string _diagramWidth = "100%";

    public void OnSplitterResized(ResizingEventArgs e)
    {
        _log.Verbose("Method called {MethodName}", nameof(OnSplitterResized));

        _diagramWidth = $"{e.PaneSize[1]}px";
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isLoaded)
        {
            _log.Verbose("Method called {MethodName}", nameof(OnAfterRenderAsync));

            _isLoaded = true;
            var windowDimensions = await _jsRuntime
                .InvokeAsync<WindowDimension>("getWindowDimensions", CancellationToken.None, null)
                .ConfigureAwait(false);

            _diagramHeight = $"{windowDimensions.Height - 60}px";
            _toolbarHeight = $"{60}px";

            await InitializePositionAndZoom().ConfigureAwait(false);
        }
    }
}
