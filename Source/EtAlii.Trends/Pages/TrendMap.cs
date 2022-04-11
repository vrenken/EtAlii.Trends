// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

public partial class TrendMap
{
    protected override void OnInitialized()
    {
        InitializeTreeView();
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var windowDimensions = await JsRuntime
            .InvokeAsync<WindowDimension>("getWindowDimensions", CancellationToken.None, null)
            .ConfigureAwait(false);

        //var toolbarHeight = _toolbar.Height;
        _diagramHeight = $"{windowDimensions.Height - 60}px";
        _toolbarHeight = $"{60}px";
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }


    public class WindowDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

}
