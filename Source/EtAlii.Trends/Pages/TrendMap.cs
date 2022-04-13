// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendMap
{
    private bool _isLoaded;

    private readonly ObservableCollection<Trend> _trends = new();

#pragma warning disable CS8618
    private Diagram _diagram;
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        await using (data.ConfigureAwait(false))
        {
            _diagram = await data.Diagrams
                .FirstAsync()
                .ConfigureAwait(false);

            await InitializeTreeView(data).ConfigureAwait(false);

            _trends.CollectionChanged += OnTrendsChanged;

            var trends = data.Trends
                .Where(t => t.Diagram == _diagram)
                .AsAsyncEnumerable()
                .ConfigureAwait(false);

            await foreach (var trend in trends)
            {
                _trends.Add(trend);
            }
        }
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

    private async Task OnClicked(ClickEventArgs e)
    {
        if (e.Count == 2 && e.ActualObject == null)
        {
            await AddNewTrend(e.Position).ConfigureAwait(false);
        }
    }

    private void OnSplitterResized(ResizingEventArgs e)
    {
        _diagramWidth = $"{e.PaneSize[1]}px";
        StateHasChanged();
    }
}
