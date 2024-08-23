using System.Reflection;
using UrlShortenerApi.Infrastructure.Database;
using Community.Microsoft.Extensions.Caching.PostgreSql;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Application.Contracts;
using UrlShortenerApi.Application.Dtos.ShortenedUrls;
using UrlShortenerApi.Application.Dtos.Users;
using UrlShortenerApi.Application.Features.ShortenedUrls.Commands;
using UrlShortenerApi.Application.Features.ShortenedUrls.Queries;
using UrlShortenerApi.Application.Features.Users.Commands;
using UrlShortenerApi.Application.Features.Users.Queries;
using UrlShortenerApi.Application.Services;

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

app.MapPost("users", async (UserCreateRequest request, IMediator mediator, CancellationToken token) =>
{
    await mediator.Send(new CreateUserCommand
    {
        FirstName = request.FirstName
    }, token);

    return Results.CreatedAtRoute();
}).WithTags("User");
app.MapGet("users/{id:guid}", async (Guid id, IMediator mediator, CancellationToken token) =>
{
    var result = await mediator.Send(new GetUserByIdQuery
    {
        UserId = id
    }, token);

    if (result.IsSuccess)
    {
        return Results.Ok(result.Value);
    }

    return Results.NotFound(result.Error.Description);
}).WithTags("User");
app.MapGet("users", async (IMediator mediator, CancellationToken token) =>
{
    var result = await mediator.Send(new GetUsersQuery(), token);

    return Results.Ok(result.Value);
}).WithTags("User");
app.MapPost("shorten",
    async (IMediator mediator, HttpContext context, ShortenUrlRequest request, CancellationToken ctx) =>
    {
        var result = await mediator.Send(new CreateShortenedUrlCommand
        {
            LongUrl = request.Url,
            Host = context.Request.Host,
            Scheme = context.Request.Scheme,
            UserId = request.UserId
        }, ctx);

        return Results.Ok(result.Value);
    }).WithTags("Shortener");
app.MapGet("c/{code}", async (string code, IMediator mediator, CancellationToken ctx) =>
{
    var result = await mediator.Send(new GetShortenedUrlQuery
    {
        Code = code
    }, ctx);

    if (result.IsSuccess)
    {
        return Results.Redirect(result.Value);
    }

    return Results.NotFound(result.Error.Description);
}).WithTags("Shortener");

app.Run();