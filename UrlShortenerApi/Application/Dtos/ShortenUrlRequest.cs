namespace UrlShortenerApi.Application.Dtos;

public record ShortenUrlRequest(string Url, Guid UserId);