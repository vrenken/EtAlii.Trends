// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public class Layer : Entity
{
    public Guid ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Expanded { get; set; }
    public bool? IsChecked { get; set; }
    public bool HasChild { get; set; }

    public Diagram Diagram { get; init; }
}
