using MediatR;

namespace UrlShortenerApi.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<Unit>
{
    public string FirstName { get; set; } = string.Empty;
}