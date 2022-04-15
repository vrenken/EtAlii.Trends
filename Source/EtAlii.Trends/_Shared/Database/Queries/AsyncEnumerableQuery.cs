// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System;

/// <summary>
/// use this base class to define async enumerable queries that do not return a result.
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract record AsyncEnumerableQuery<THandler>() : AsyncEnumerableQuery(typeof(THandler))
    where THandler : IAsyncEnumerableQueryHandler;

/// <summary>
/// Do not use this base class - it should only be used by the dispatcher itself.
/// </summary>
public abstract record AsyncEnumerableQuery(Type HandlerType);

