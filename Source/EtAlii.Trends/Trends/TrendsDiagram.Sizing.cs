// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Trends;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendsDiagram
{
    // Specify the orientation of the layout.
    private LayoutOrientation _orientation = LayoutOrientation.TopToBottom;
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Auto;
    private VerticalAlignment _verticalAlignment = VerticalAlignment.Auto;
    private int _horizontalSpacing = 30;
    private int _verticalSpacing = 30;

    private bool _isLoaded;
    private string _diagramHeight = "900px";
    private string _diagramWidth = "100%";

    public void OnSplitterResized(ResizingEventArgs e)
    {
        _diagramWidth = $"{e.PaneSize[1]}px";
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isLoaded)
        {
            _isLoaded = true;
            var windowDimensions = await _jsRuntime
                .InvokeAsync<WindowDimension>("getWindowDimensions", CancellationToken.None, null)
                .ConfigureAwait(false);

            //var toolbarHeight = _toolbar.Height;
            _diagramHeight = $"{windowDimensions.Height - 60}px";
            _toolbarHeight = $"{60}px";
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }
}
