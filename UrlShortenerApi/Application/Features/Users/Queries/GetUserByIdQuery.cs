using MediatR;
using UrlShortenerApi.Application.Dtos.Users;

namespace UrlShortenerApi.Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<UserResponse>
{
    public Guid UserId { get; set; }
}