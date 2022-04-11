// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

public partial class TrendMap
{
    private readonly List<Layer> _layers = new();

    private void InitializeTreeView()
    {
        _layers.Add(new Layer
        {
            Id = 1,
            Name = "Discover Music",
            HasChild = true,
        });
        _layers.Add(new Layer
        {
            Id = 2,
            ParentId = 1,
            Name = "Hot Singles"
        });
        _layers.Add(new Layer
        {
            Id = 3,
            ParentId = 1,
            Name = "Rising Artists"
        });
        _layers.Add(new Layer
        {
            Id = 4,
            ParentId = 1,
            Name = "Live Music"
        });
        _layers.Add(new Layer
        {
            Id = 14,
            HasChild = true,
            Name = "MP3 Albums",
            Expanded = true,
            IsChecked = true
        });
        _layers.Add(new Layer
        {
            Id = 15,
            ParentId = 14,
            Name = "Rock"
        });
        _layers.Add(new Layer
        {
            Id = 16,
            Name = "Gospel",
            ParentId = 14,
        });
        _layers.Add(new Layer
        {
            Id = 17,
            ParentId = 14,
            Name = "Latin Music"
        });
        _layers.Add(new Layer
        {
            Id = 18,
            ParentId = 14,
            Name = "Jazz"
        });
    }
}
