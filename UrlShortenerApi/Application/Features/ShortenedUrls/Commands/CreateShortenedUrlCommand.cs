using MediatR;
using UrlShortenerApi.Application.Dtos.ShortenedUrls;
using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Commands;

public class CreateShortenedUrlCommand : IRequest<Result<ShortenUrlResponse>>
{
    public string LongUrl { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string Scheme { get; init; } = string.Empty;
    public HostString Host { get; init; }
}