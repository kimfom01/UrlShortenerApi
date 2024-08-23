using MediatR;
using UrlShortenerApi.Application.Dtos.Users;
using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Features.Users.Queries;

public class GetUsersQuery : IRequest<Result<List<UserResponse>>>
{
}