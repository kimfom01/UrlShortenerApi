using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortenerApi.Application.Dtos.Users;
using UrlShortenerApi.Application.Errors;
using UrlShortenerApi.Domain;
using UrlShortenerApi.Domain.Abstractions;
using UrlShortenerApi.Infrastructure.Cache;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly UrlShortenerContext _context;
    private readonly IDistributedCache _cache;

    public GetUserByIdQueryHandler(UrlShortenerContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken token)
    {
        var user = await _cache.GetOrCreateAsync<User?>($"users-{request.UserId}", async cToken =>
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cToken);

            return user;
        }, token: token);

        if (user is null)
        {
            return Result<UserResponse>.Failure(UserResponseErrors.NotFound);
        }

        return Result<UserResponse>.Success(new UserResponse(user.Id, user.FirstName));
    }
}