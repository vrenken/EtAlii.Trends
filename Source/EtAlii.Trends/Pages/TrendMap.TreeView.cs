// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Navigations;

public partial class TrendMap
{
#pragma warning disable CS8618
    private SfTreeView<Layer> _layersTreeView;
    private string _selectedLayerNodeId;
#pragma warning restore CS8618

    private string[] _checkedLayerNodes = Array.Empty<string>();
    private string[] _expandedLayerNodes = Array.Empty<string>();

    private void OnLayerNodeEditing()
    {
        //_layerTreeViewMenuItems.ForEach(mi => mi.Disabled = true);
    }

    private async Task OnLayerNodeEdited(NodeEditEventArgs e)
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

        //_layerTreeViewMenuItems.ForEach(mi => mi.Disabled = false);
    }

    private void OnLayerNodeSelect(NodeSelectEventArgs e) => _selectedLayerNodeId = e.NodeData.Id;
    private void OnLayerNodeClicked(NodeClickEventArgs e) => _selectedLayerNodeId = e.NodeData.Id;

    private async Task OnLayerNodeExpandedChanged(NodeExpandEventArgs e)
    {
        var layerId = Guid.Parse(e.NodeData.Id);

        var layer = _layers.Single(l => l.Id == layerId);
        await _commandDispatcher
            .DispatchAsync<Layer>(new UpdateLayerCommand(layer))
            .ConfigureAwait(false);
    }

    private async Task OnLayerNodeCheckedChanged(NodeCheckEventArgs e)
    {
        var layerId = Guid.Parse(e.NodeData.Id);

        var layer = _layers.Single(l => l.Id == layerId);

        await _commandDispatcher
            .DispatchAsync<Layer>(new UpdateLayerCommand(layer))
            .ConfigureAwait(false);
    }
}
