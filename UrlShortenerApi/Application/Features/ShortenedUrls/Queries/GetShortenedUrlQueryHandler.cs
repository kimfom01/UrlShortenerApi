using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortenerApi.Application.Errors;
using UrlShortenerApi.Domain.Abstractions;
using UrlShortenerApi.Infrastructure.Cache;
using UrlShortenerApi.Infrastructure.Database;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Queries;

public class GetShortenedUrlQueryHandler : IRequestHandler<GetShortenedUrlQuery, Result<string>>
{
    private readonly UrlShortenerContext _context;
    private readonly IDistributedCache _cache;

    public GetShortenedUrlQueryHandler(UrlShortenerContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<string>> Handle(GetShortenedUrlQuery request, CancellationToken ctx)
    {
        var shortenedUrl = await _cache.GetOrCreateAsync($"{request.Code}",
            async token => await _context.ShortenedUrls.SingleOrDefaultAsync(sh
                    => sh.Code == request.Code,
                token),
            token: ctx);

        if (shortenedUrl is null || string.IsNullOrWhiteSpace(shortenedUrl.LongUrl))
        {
            return Result<string>.Failure(ShortenedUrlErrors.NotFound);
        }

        return Result<string>.Success(shortenedUrl.LongUrl);
    }
}