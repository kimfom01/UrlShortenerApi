using MediatR;
using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Queries;

public class GetShortenedUrlQuery : IRequest<Result<string>>
{
    public string Code { get; init; } = string.Empty;
}