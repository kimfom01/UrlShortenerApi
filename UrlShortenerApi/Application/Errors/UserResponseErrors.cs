using UrlShortenerApi.Domain.Abstractions;

namespace UrlShortenerApi.Application.Errors;

public static class UserResponseErrors
{
    public static Error NotFound => new("User could not be found");
}