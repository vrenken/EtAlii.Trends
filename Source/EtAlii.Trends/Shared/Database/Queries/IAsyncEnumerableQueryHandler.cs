// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public interface IAsyncEnumerableQueryHandler
{
    IAsyncEnumerable<object> Handle(AsyncEnumerableQuery query);
}
