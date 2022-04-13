// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public interface IAsyncEnumerableQueryHandler<in TQuery, out TResult> : IAsyncEnumerableQueryHandler
    where TQuery: AsyncEnumerableQuery
{
    async IAsyncEnumerable<object> IAsyncEnumerableQueryHandler.Handle(AsyncEnumerableQuery command)
    {
        var items = Handle((TQuery)command).ConfigureAwait(false);
        await foreach (var item in items)
        {
            yield return item!;
        }
    }
    IAsyncEnumerable<TResult> Handle(TQuery query);
}
