using MediatR;
using UrlShortenerApi.Domain;
using UrlShortenerApi.Domain.Abstractions;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Unit>>
{
    private readonly UrlShortenerContext _context;

    public CreateUserCommandHandler(UrlShortenerContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(CreateUserCommand request, CancellationToken token)
    {
        var user = new User
        {
            FirstName = request.FirstName
        };

        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);

        return Result<Unit>.Success(Unit.Value);
    }
}