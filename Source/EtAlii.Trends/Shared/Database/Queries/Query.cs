// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System;

#pragma warning disable CA1724

/// <summary>
/// use this base class to define non-async queries that do not return a result.
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class Query<THandler> : Query
    where THandler : IQueryHandler
{
    protected Query() : base(typeof(THandler))
    {
    }
}

/// <summary>
/// Do not use this base class - it should only be used by the dispatcher itself.
/// </summary>
public abstract class Query
{
    public Type HandlerType { get; }

    protected Query(Type handlerType)
    {
        HandlerType = handlerType;
    }
}
