// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Layers;

using Syncfusion.Blazor.Navigations;

public partial class LayersTreeView
{

#pragma warning disable CS8618
    private SfContextMenu<MenuItem> _layerTreeViewMenu;
#pragma warning restore CS8618

    // Datasource for menu items
    private readonly List<MenuItem> _layerTreeViewMenuItems = new()
    {
        new MenuItem { Text = "Edit" },
        new MenuItem { Text = "Remove" },
        new MenuItem { Text = "Add" }
    };


    // Triggers when context menu is selected
    private async Task OnLayerTreeViewMenuItemSelect(MenuEventArgs<MenuItem> args)
    {
        var selectedMenuItem = args.Item.Text;
        switch (selectedMenuItem)
        {
            case "Edit":
                await _layersTreeView
                    .BeginEdit(_selectedLayerNodeId)
                    .ConfigureAwait(false);
                break;
            case "Remove":
                await RemoveNodes().ConfigureAwait(false);
                break;
            case "Add":
                await AddNodes().ConfigureAwait(false);
                break;
        }
    }

    private async Task AddNodes()
    {
        // Expand the selected nodes
        var parentId = Guid.Parse(_selectedLayerNodeId);

        var parentLayer = _layers.Single(l => l.Id == parentId);

        var command = new AddLayerCommand
        (
            Layer: (diagram, parent) => new Layer
            {
                Name = $"New layer {_layers.Count + 1}",
                Diagram = diagram,
                Parent = parent,
                IsChecked = true,
            },
            DiagramId: DiagramId,
            ParentLayerId: parentId
        );
        var layer = await _commandDispatcher
            .DispatchAsync<Layer>(command)
            .ConfigureAwait(false);

        parentLayer.Children.Add(layer);
        parentLayer.IsExpanded = true;

        await _commandDispatcher
            .DispatchAsync<Layer>(new UpdateLayerCommand(parentLayer))
            .ConfigureAwait(false);

        _layers.Add(layer);

        _layersTreeView.AddNodes(new() {layer}, parentId.ToString());

        _checkedLayerNodes = _checkedLayerNodes
            .Concat(_layers.Where(l => l.IsChecked).Select(l => l.Id.ToString()))
            //.Where(l => l != null)
            .Distinct()
            .ToArray();

        _expandedLayerNodes = _expandedLayerNodes
            .Concat(new[] { parentId.ToString() })
            //.Where(l => l != null)
            .Distinct()
            .ToArray();

        _selectedLayerNodeId = layer.Id.ToString();

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

        var layer = _layers.Single(l => l.Id == layerId);
        layer.IsChecked = false;

        await _commandDispatcher
            .DispatchAsync(new RemoveLayerCommand(layerId))
            .ConfigureAwait(false);

        _layers.Remove(layer);
    }
}
