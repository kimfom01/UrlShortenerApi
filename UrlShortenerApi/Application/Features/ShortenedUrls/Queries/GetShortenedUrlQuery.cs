using MediatR;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Queries;

public class GetShortenedUrlQuery : IRequest<string?>
{
    public string Code { get; set; } = string.Empty;
}