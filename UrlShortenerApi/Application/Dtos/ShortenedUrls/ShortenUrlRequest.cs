namespace UrlShortenerApi.Application.Dtos.ShortenedUrls;

public record ShortenUrlRequest(string Url, Guid UserId);