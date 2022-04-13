// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Threading.Tasks;

public interface IAsyncQueryHandler<in TQuery, TResult> : IAsyncQueryHandler
    where TQuery: AsyncQuery
{
    Task<object> IAsyncQueryHandler.Handle(AsyncQuery command)
    {
        var task = Task.Run(async () => await Handle((TQuery)command).ConfigureAwait(false));
        var result = task.GetAwaiter().GetResult()!;
        return Task.FromResult<object>(result);
    }
    Task<TResult> Handle(TQuery query);
}
