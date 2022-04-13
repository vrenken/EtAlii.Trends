// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System.Threading.Tasks;

public interface IAsyncQueryHandler
{
    Task<object> Handle(AsyncQuery query);
}
