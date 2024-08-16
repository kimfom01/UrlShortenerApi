using System.Reflection;
using UrlShortenerApi.Domain;
using UrlShortenerApi.Infrastructure.Database;
using Community.Microsoft.Extensions.Caching.PostgreSql;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortenerApi.Application.Contracts;
using UrlShortenerApi.Application.Dtos;
using UrlShortenerApi.Application.Features.ShortenedUrls.Commands;
using UrlShortenerApi.Application.Features.ShortenedUrls.Queries;
using UrlShortenerApi.Application.Services;
using UrlShortenerApi.Infrastructure.Cache;

var builder = WebApplication.CreateBuilder(args);

const string connectionString = "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=AdminDb;";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICodeGenerator, CodeGenerator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddDbContext<UrlShortenerContext>(options => { options.UseNpgsql(connectionString); });
builder.Services.AddCoreAdmin();
builder.Services.AddDistributedPostgreSqlCache(setup =>
{
    setup.ConnectionString = connectionString;
    setup.SchemaName = "cache-schema";
    setup.TableName = "cache-table";
    setup.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.MapPost("users", async (User user, UrlShortenerContext context) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();

    return Results.Created();
});
app.MapGet("users/{id:guid}",
    async (Guid id, UrlShortenerContext context, IDistributedCache cache, CancellationToken ctx) =>
    {
        var user = await cache.GetOrCreateAsync<User?>($"users-{id}", async token =>
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, token);

            return user;
        }, token: ctx);

        if (user is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(user);
    });
app.MapGet("users", (UrlShortenerContext context) => Results.Ok(context.Users));
app.MapPost("shorten",
    async (IMediator mediator, HttpContext context, ShortenUrlRequest request, CancellationToken ctx) =>
    {
        var shortenedUrl = await mediator.Send(new CreateShortenedUrlCommand
        {
            LongUrl = request.Url,
            Host = context.Request.Host,
            Scheme = context.Request.Scheme,
            UserId = request.UserId
        }, ctx);

        return Results.Ok(shortenedUrl);
    });
app.MapGet("c/{code}", async (string code, IMediator mediator, CancellationToken ctx) =>
{
    var longUrl = await mediator.Send(new GetShortenedUrlQuery
    {
        Code = code
    }, ctx);

    if (string.IsNullOrWhiteSpace(longUrl))
    {
        return Results.NotFound();
    }

    return Results.Redirect(longUrl);
});

app.Run();