# Url Shortener API

This is a simple implementation of a URL shortener.

## Features
- Url shortener
- Caching (distributed cache using postgres)
- Storage (postgres)
- User accounts
- Admin dashboard

## Usage

### Requirements

- .NET 8
- Postgres

### Running

- Update the `connectionString` variable in Program.cs
- On the terminal navigate to `UrlShortenerApi/UrlShortenerApi` directory (the same level where you can find `UrlShortenerApi.csproj`)
- Apply migrations to update the database
```sh
dotnet ef database update
```
- Run the following to start the API 
```sh
dotnet run
```
- To see the swagger documentation, visit [http://localhost:5174/swagger/index.html](http://localhost:5174/swagger/index.html)
- To see the admin dashboard, visit [http://localhost:5174/coreadmin](http://localhost:5174/coreadmin)