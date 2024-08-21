using MediatR;
using UrlShortenerApi.Application.Dtos.ShortenedUrls;

namespace UrlShortenerApi.Application.Features.ShortenedUrls.Commands;

public class CreateShortenedUrlCommand : IRequest<ShortenUrlResponse>
{
    public string LongUrl { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Scheme { get; set; } = string.Empty;
    public HostString Host { get; set; }
}