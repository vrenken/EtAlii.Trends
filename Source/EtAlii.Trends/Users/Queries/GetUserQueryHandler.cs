// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

public record GetUserQuery(Guid UserId) : AsyncQuery<IGetUserQueryHandler>;

public interface IGetUserQueryHandler : IAsyncQueryHandler<GetUserQuery, User> {}

public class GetUserQueryHandler : IGetUserQueryHandler
{
    public async Task<User> Handle(GetUserQuery query)
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        return await data.Users
            .AsNoTracking()
            //.Where(u => u.Id == query.UserId)
            .SingleAsync()
            .ConfigureAwait(false);
    }
}


