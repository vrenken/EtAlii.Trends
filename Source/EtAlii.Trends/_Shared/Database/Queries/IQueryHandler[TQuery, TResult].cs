// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler
    where TQuery: Query
{
    object IQueryHandler.Handle(Query query) => Handle((TQuery)query)!;

    TResult Handle(TQuery query);
}
