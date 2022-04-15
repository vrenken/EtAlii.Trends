// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Threading.Tasks;

public interface IQueryDispatcher
{
    /// <summary>
    /// Dispatch a non-async query, without returning any results.
    /// </summary>
    /// <param name="query"></param>
    TResult Dispatch<TResult>(Query query);

    /// <summary>
    /// Dispatch an async query that returns a result.
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> DispatchAsync<TResult>(AsyncQuery query);

    /// <summary>
    /// Dispatch an async enumerable query that returns a result.
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    IAsyncEnumerable<TResult> DispatchAsync<TResult>(AsyncEnumerableQuery query);

}
