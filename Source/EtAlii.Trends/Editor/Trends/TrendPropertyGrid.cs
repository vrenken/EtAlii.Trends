// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using Microsoft.AspNetCore.Components;

public partial class TrendPropertyGrid
{
    private List<Component> _components = new();

    [Parameter] public Trend? Trend { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _components.Add(new Component
        {
            Name = "Inbox",
        });
        _components.Add(new Component
        {
            Name = "Categories",
        });
        _components.Add(new Component
        {
            Name = "Primary"
        });
        _components.Add(new Component
        {
            Name = "Social"
        });
        _components.Add(new Component
        {
            Name = "Promotions"
        });
    }
}
