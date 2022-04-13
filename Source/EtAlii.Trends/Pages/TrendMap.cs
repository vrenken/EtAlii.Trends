// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using System.Collections.ObjectModel;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Layouts;

public partial class TrendMap
{
    private bool _isLoaded;

    private readonly ObservableCollection<Trend> _trends = new();

    private Guid _diagramId;

    protected override async Task OnInitializedAsync()
    {
        var user = await _queryDispatcher.DispatchAsync<User>(new GetUserQuery(Guid.Empty)).ConfigureAwait(false);
        // var diagrams = _queryDispatcher
        //     .DispatchAsync<Diagram>(new GetAllDiagramsForUserQuery(user.Id))
        //     .ConfigureAwait(false);
        var diagram = await _queryDispatcher.DispatchAsync<Diagram>(new GetDiagramQuery(Guid.Empty)).ConfigureAwait(false);

        _diagramId = diagram.Id;

        await InitializeLayers().ConfigureAwait(false);

        _trends.CollectionChanged += OnTrendsChanged;

        var trends = _queryDispatcher
            .DispatchAsync<Trend>(new GetAllTrendsQuery(_diagramId))
            .ConfigureAwait(false);
        await foreach (var trend in trends)
        {
            _trends.Add(trend);
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
