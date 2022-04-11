// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using System.Collections.ObjectModel;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendMap
{
    private bool _isLoaded;

    private readonly ObservableCollection<Trend> _trends = new();

    protected override void OnInitialized()
    {
        InitializeTreeView();
        _trends.CollectionChanged += OnTrendsChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isLoaded)
        {
            _isLoaded = true;
            var windowDimensions = await JsRuntime
                .InvokeAsync<WindowDimension>("getWindowDimensions", CancellationToken.None, null)
                .ConfigureAwait(false);

            //var toolbarHeight = _toolbar.Height;
            _diagramHeight = $"{windowDimensions.Height - 60}px";
            _toolbarHeight = $"{60}px";
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }

    public class WindowDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private void OnClicked(ClickEventArgs e)
    {
        if (e.Count == 2 && e.ActualObject == null)
        {
            AddNewTrend(e.Position);
        }
    }

    private void OnSplitterResized(ResizingEventArgs e)
    {
        _diagramWidth = $"{e.PaneSize[1]}px";
        StateHasChanged();
    }
}
