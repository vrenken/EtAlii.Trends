// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Navigations;

public partial class TrendMap
{

#pragma warning disable CS8618
    private SfContextMenu<MenuItem> _layerTreeViewMenu;
#pragma warning restore CS8618

    // Datasource for menu items
    private readonly List<MenuItem> _layerTreeViewMenuItems = new(){
        new MenuItem { Text = "Edit" },
        new MenuItem { Text = "Remove" },
        new MenuItem { Text = "Add" }
    };


    // Triggers when context menu is selected
    private async Task OnLayerTreeViewMenuItemSelect(MenuEventArgs<MenuItem> args)
    {
        var selectedMenuItem = args.Item.Text;
        if (selectedMenuItem == "Edit")
        {
            await _layersTreeView
                .BeginEdit(_selectedLayerNodeId)
                .ConfigureAwait(false);

        }
        else if (selectedMenuItem == "Remove")
        {
            await RemoveNodes().ConfigureAwait(false);
        }
        else if (selectedMenuItem == "Add")
        {
            await AddNodes().ConfigureAwait(false);
        }
    }

    private async Task AddNodes()
    {
        // Expand the selected nodes
        var parentId = Guid.Parse(_selectedLayerNodeId);

        var parentLayer = _layers.Single(l => l.Id == parentId);

        // _expandedLayerNodes = _expandedLayerNodes
        //     .Concat(new[] { _selectedLayerNodeId })
        //     .Distinct()
        //     .ToArray();

        var command = new AddLayerCommand
        (
            Layer: (diagram, parent) => new Layer
            {
                Name = $"New layer {_layers.Count + 1}",
                Diagram = diagram,
                Parent = parent,
                IsChecked = true,
            },
            DiagramId: _diagramId,
            ParentLayerId: parentId
        );
        var layer = await _commandDispatcher
            .DispatchAsync<Layer>(command)
            .ConfigureAwait(false);

        parentLayer.Children.Add(layer);
        parentLayer.Expanded = true;

        _selectedLayerNodeId = layer.Id.ToString();
        _layers.Add(layer);

        //_layersTreeView.ExpandedNodes = _layersTreeView.ExpandedNodes.Concat(new[] { _selectedLayerNodeId }).Distinct().ToArray();

        // Edit the added node.
        await Task.Delay(100).ConfigureAwait(false);
        await _layersTreeView
            .BeginEditAsync(_selectedLayerNodeId)
            .ConfigureAwait(false);
    }

    // To delete a tree node
    private async Task RemoveNodes()
    {
        var layerId = Guid.Parse(_selectedLayerNodeId);

        await _commandDispatcher
            .DispatchAsync(new RemoveLayerCommand(layerId))
            .ConfigureAwait(false);

        var layer = _layers.Single(l => l.Id == layerId);
        _layers.Remove(layer);
    }
}
