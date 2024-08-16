using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Application.Services;
using UrlShortenerApi.Domain;

namespace UrlShortenerApi.Infrastructure.Database;

public class UrlShortenerContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(us => us.FirstName)
                .IsUnique();
        });

        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(sho => sho.Code)
                .HasMaxLength(ShortLinkSettings.Length);

            builder.HasIndex(sho => sho.Code)
                .IsUnique();
        });
    }
}