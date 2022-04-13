// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public class Layer : DiagramEntity
{
    public Layer? Parent { get; set; }
    public Guid? ParentId { get; protected set; }

    public IList<Layer> Children { get; protected set; } = new List<Layer>();
    public string Name { get; set; } = string.Empty;
    public bool Expanded { get; set; }
    public bool? IsChecked { get; set; }
    public bool HasChild => Children.Any();
}
