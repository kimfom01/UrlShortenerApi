using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Errors;

public static class ShortenedUrlErrors
{
    public static Error NotFound => new("ShortenedUrl could not be found");
}