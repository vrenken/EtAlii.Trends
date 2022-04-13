// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Microsoft.EntityFrameworkCore;

public partial class TrendMap
{
    private readonly List<Layer> _layers = new();

    private async Task InitializeTreeView(DataContext dataContext)
    {
        var layers = dataContext.Layers
            .Where(l => l.Diagram == _diagram)
            .AsAsyncEnumerable()
            .ConfigureAwait(false);

        await foreach (var layer in layers)
        {
            _layers.Add(layer);
        }
    }
}
