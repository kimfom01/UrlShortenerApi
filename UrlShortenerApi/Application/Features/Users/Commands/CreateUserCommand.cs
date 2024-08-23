using MediatR;
using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<Result<Unit>>
{
    public string FirstName { get; init; } = string.Empty;
}