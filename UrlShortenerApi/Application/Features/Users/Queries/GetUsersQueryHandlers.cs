using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Application.Dtos.Users;
using UrlShortenerApi.Domain.Abstractions;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.Users.Queries;

public class GetUsersQueryHandlers : IRequestHandler<GetUsersQuery, Result<List<UserResponse>>>
{
    private readonly UrlShortenerContext _context;

    public GetUsersQueryHandlers(UrlShortenerContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery request, CancellationToken token)
    {
        var users = _context.Users.AsNoTracking();

        var userResponse = await users.Select(usr => new UserResponse(usr.Id, usr.FirstName)).ToListAsync(token);

        return Result<List<UserResponse>>.Success(userResponse);
    }
}