// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

public partial class TrendMap
{
    private readonly List<Layer> _layers = new();

    private async Task InitializeLayers()
    {
        var query = new GetAllLayersQuery(_diagramId);
        var layers = _queryDispatcher
            .DispatchAsync<Layer>(query)
            .ConfigureAwait(false);

        await foreach (var layer in layers)
        {
            _layers.Add(layer);
        }
    }

}
