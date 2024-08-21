using MediatR;
using UrlShortenerApi.Domain;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly UrlShortenerContext _context;

    public CreateUserCommandHandler(UrlShortenerContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken token)
    {
        var user = new User
        {
            FirstName = request.FirstName
        };

        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);

        return Unit.Value;
    }
}