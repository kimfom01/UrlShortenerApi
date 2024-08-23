namespace UrlShortenerApi.Domain.Abstractions;

public record Error(string? Description = null)
{
    public static readonly Error None = new();
}