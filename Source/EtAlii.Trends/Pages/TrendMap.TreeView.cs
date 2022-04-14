// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Navigations;

public partial class TrendMap
{
    private async Task OnCheckedNodeChanged(string[] ids)
    {
        foreach (var id in ids)
        {
            var layerId = Guid.Parse(id);

            var layer = _layers.Single(l => l.Id == layerId);
            await _commandDispatcher
                .DispatchAsync<Layer>(new UpdateLayerCommand(layer))
                .ConfigureAwait(false);
        }
    }

    private async Task OnExpandedNodesChanged(string[] ids)
    {
        foreach (var id in ids)
        {
            var layerId = Guid.Parse(id);

            var layer = _layers.Single(l => l.Id == layerId);
            await _commandDispatcher
                .DispatchAsync<Layer>(new UpdateLayerCommand(layer))
                .ConfigureAwait(false);
        }
    }

    private async Task OnNodeEdited(NodeEditEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewText))
        {
            var id = e.NodeData.Id;
            var layerId = Guid.Parse(id);

            var layer = _layers.Single(l => l.Id == layerId);
            layer.Name = e.NewText;

            await _commandDispatcher
                .DispatchAsync<Layer>(new UpdateLayerCommand(layer))
                .ConfigureAwait(false);
        }
        else
        {
            e.Cancel = true;
        }
    }
}
