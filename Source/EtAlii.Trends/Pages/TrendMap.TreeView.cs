// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

public partial class TrendMap
{
    private readonly List<Layer> _layers = new();

    private void InitializeTreeView()
    {
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            Name = "Discover Music",
            HasChild = true,
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[0].Id,
            Name = "Hot Singles"
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[0].Id,
            Name = "Rising Artists"
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[0].Id,
            Name = "Live Music"
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            HasChild = true,
            Name = "MP3 Albums",
            Expanded = true,
            IsChecked = true
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[3].Id,
            Name = "Rock"
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            Name = "Gospel",
            ParentId = _layers[3].Id,
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[3].Id,
            Name = "Latin Music"
        });
        _layers.Add(new Layer
        {
            Id = Guid.NewGuid(),
            ParentId = _layers[3].Id,
            Name = "Jazz"
        });
    }
}
