// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

public partial class TrendPropertyGrid
{
    private IList<Component>? _components;

    [CascadingParameter(Name = "SelectedTrend")] public Trend? SelectedTrend { get; set; }

    [Parameter] public EventCallback<Trend?> SelectedTrendChanged { get; set; }

    private DateTime? _selectedTrendBegin;
    private DateTime? _selectedTrendEnd;

    protected override Task OnParametersSetAsync()
    {
        if (SelectedTrend != null)
        {
            _selectedTrendBegin = SelectedTrend.Begin;
            _selectedTrendEnd = SelectedTrend.End;
            _components = SelectedTrend.Components;
        }
        else
        {
            _selectedTrendBegin = null;
            _selectedTrendEnd = null;
            _components = null;
        }

        return Task.CompletedTask;
    }

    private async Task UpdateSelectedTrendBegin(DateTime? begin)
    {
        if (SelectedTrend != null)
        {
            SelectedTrend.Begin = begin ?? DateTime.Now;

            await _commandDispatcher
                .DispatchAsync<Trend>(new UpdateTrendCommand(SelectedTrend!))
                .ConfigureAwait(false);
        }

        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
    }

    private async Task UpdateSelectedTrendEnd(DateTime? end)
    {
        if (SelectedTrend != null)
        {
            SelectedTrend.End = end ?? DateTime.Now;

            await _commandDispatcher
                .DispatchAsync<Trend>(new UpdateTrendCommand(SelectedTrend))
                .ConfigureAwait(false);
        }

        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
    }

    private async Task OnActionComplete(ActionEventArgs<Component> e)
    {
        if (SelectedTrend == null)
        {
            e.Cancel = true;
            return;
        }

        var component = e.Data;
        if (component == null)
        {
            e.Cancel = true;
            return;
        }

        switch (e.RequestType)
        {
            case Action.Save:
                var isNew = component.Id == Guid.Empty;
                if (isNew)
                {
                    await AddComponent(component).ConfigureAwait(false);
                }
                else
                {
                    await UpdateComponent(component).ConfigureAwait(false);
                }
                break;
            case Action.Delete:
                await RemoveComponent(component).ConfigureAwait(false);
                break;
        }
    }

    private async Task RemoveComponent(Component component)
    {
        var command = new RemoveComponentCommand(component.Id);
        await _commandDispatcher
            .DispatchAsync(command)
            .ConfigureAwait(false);

        _components!.Remove(component);

        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
}

    private async Task UpdateComponent(Component component)
    {
        var command = new UpdateComponentCommand(component);
        await _commandDispatcher
            .DispatchAsync<Component>(command)
            .ConfigureAwait(false);

        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
    }

    private async Task AddComponent(Component component)
    {
        var componentToAdd = component;
        var command = new AddComponentCommand
        (
            Component: trend =>
            {
                componentToAdd.Trend = trend;
                return componentToAdd;
            },
            TrendId: SelectedTrend!.Id
        );
        await _commandDispatcher
            .DispatchAsync<Component>(command)
            .ConfigureAwait(false);

        await SelectedTrendChanged.InvokeAsync(SelectedTrend).ConfigureAwait(false);
    }
}
