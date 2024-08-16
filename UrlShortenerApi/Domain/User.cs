using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    public ICollection<ShortenedUrl>? ShortenedUrls { get; set; }
}