// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

/// <inheritdoc />
public class QueryDispatcher : IQueryDispatcher
{
    private const int DebugLockTimeout = 10000;

    private readonly Func<Type, object> _queryHandlerResolver;
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    private readonly ILogger _log = Log.ForContext<QueryDispatcher>();
    public QueryDispatcher(IServiceProvider serviceProvider) // Func<Type, object> queryHandlerResolver
    {
        _queryHandlerResolver = type => serviceProvider.GetService(type)!; // queryHandlerResolver;
    }

    /// <inheritdoc />
    public TResult Dispatch<TResult>(Query query)
    {
        _log
            .ForContext("QueryName", query.GetType().Name)
            .Debug("Handling query");

        TResult result;

        _lock.Wait();
        try
        {
            var handler = (IQueryHandler)_queryHandlerResolver(query.HandlerType);
            result = (TResult)handler.Handle(query);
        }
        finally
        {
            _lock.Release();
        }

        _log
            .ForContext("QueryName", query.GetType().Name)
            .Verbose("Handled query");

        return result;
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TResult>(AsyncQuery query)
    {
        _log
            .ForContext("QueryName", query.GetType().Name)
            .Debug("Handling query");

        TResult result;
#if DEBUG
        var success = await _lock.WaitAsync(DebugLockTimeout).ConfigureAwait(false);
        if (!success)
        {
            _log.Error("Query dispatcher locked on executing command {QueryName}", query.GetType().Name);
            await _lock.WaitAsync().ConfigureAwait(false);
        }
#else
        await _lock.WaitAsync();
#endif

        try
        {
            var handler = (IAsyncQueryHandler)_queryHandlerResolver(query.HandlerType);
            result = (TResult)await handler
                .Handle(query)
                .ConfigureAwait(false);

        }
        finally
        {
            _lock.Release();
        }

        _log
            .ForContext("QueryName", query.GetType().Name)
            .Debug("Handled query");

        return result;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<TResult> DispatchAsync<TResult>(AsyncEnumerableQuery query)
    {
        _log
            .ForContext("QueryName", query.GetType().Name)
            .Debug("Handling query");

#if DEBUG
        var success = await _lock.WaitAsync(DebugLockTimeout).ConfigureAwait(false);
        if (!success)
        {
            _log.Error("Query dispatcher locked on executing command {QueryName}", query.GetType().Name);
            await _lock.WaitAsync().ConfigureAwait(false);
        }
#else
        await _lock.WaitAsync();
#endif

        try
        {
            var handler = (IAsyncEnumerableQueryHandler)_queryHandlerResolver(query.HandlerType);
            var items = handler
                .Handle(query)
                .ConfigureAwait(false);
            await foreach (var item in items)
            {
                yield return (TResult)item;
            }

        }
        finally
        {
            _lock.Release();
        }

        _log
            .ForContext("QueryName", query.GetType().Name)
            .Debug("Handled query");
    }
}
