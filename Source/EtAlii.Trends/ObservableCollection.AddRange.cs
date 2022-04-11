// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Collections.ObjectModel;

public static class ObservableCollectionAddRangeExtension
{
    public static void AddRange<T>(this ObservableCollection<T> collection, T[] items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
};
