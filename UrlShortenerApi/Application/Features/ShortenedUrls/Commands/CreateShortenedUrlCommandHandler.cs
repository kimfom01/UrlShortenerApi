using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Application.Contracts;
using UrlShortenerApi.Application.Dtos.ShortenedUrls;
using UrlShortenerApi.Domain;
using UrlShortenerApi.Domain.Abstractions;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Commands;

public class CreateShortenedUrlCommandHandler : IRequestHandler<CreateShortenedUrlCommand, Result<ShortenUrlResponse>>
{
    private readonly UrlShortenerContext _context;
    private readonly ICodeGenerator _codeGenerator;

    public CreateShortenedUrlCommandHandler(UrlShortenerContext context, ICodeGenerator codeGenerator)
    {
        _context = context;
        _codeGenerator = codeGenerator;
    }

    public async Task<Result<ShortenUrlResponse>> Handle(CreateShortenedUrlCommand request, CancellationToken ctx)
    {
        string code;
        do
        {
            code = _codeGenerator.GenerateUniqueCode();
        } while (await _context.ShortenedUrls.AnyAsync(s => s.Code == code, ctx));

        var shortenedUrl = new ShortenedUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = request.LongUrl,
            Code = code,
            CreatedOnUtc = DateTime.UtcNow,
            ShortUrl = $"{request.Scheme}://{request.Host}/c/{code}",
            UserId = request.UserId
        };

        var addedEntry = await _context.ShortenedUrls.AddAsync(shortenedUrl, ctx);
        await _context.SaveChangesAsync(ctx);

        return Result<ShortenUrlResponse>.Success(new ShortenUrlResponse(addedEntry.Entity.ShortUrl));
    }
}