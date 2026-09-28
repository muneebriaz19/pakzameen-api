# PakZameen API

The backend of a property listing platform, written in C# with ASP.NET Core and
Entity Framework Core. Built in 2023 as a side project with friends, during my first
year working as a developer. I wrote the backend. It is published here as a record of
that work.

## Structure

The solution is split into four projects:

- **Domain**: the entities and the core model, with no dependencies on anything else
- **Application**: the data context, DTOs, interfaces, services, validators, EF Core
  migrations and seed data
- **Infrastructure**: the supporting implementations the application layer depends on
- **PakZameen_Api**: the ASP.NET Core Web API itself, with the controllers,
  authorisation and request filters

The dependencies point inwards, so the domain knows nothing about the API and the
controllers stay thin, with the work happening in the service layer. Requests come in
as DTOs and are checked by validators before anything reaches the database.

## What is built in

Besides the property and user handling, the backend covers the parts that a listing
site needs around them: account verification and password reset by email with
one-time codes and expiring tokens, request authorisation, validation on incoming
data, database migrations and seeding, and an Excel import for bulk data.

## Running it

You need .NET and SQL Server or LocalDB.

```
git clone https://github.com/muneebriaz19/pakzameen-api.git
cd pakzameen-api
dotnet restore
dotnet ef database update --project Application --startup-project PakZameen_Api
dotnet run --project PakZameen_Api
```

The configuration in `appsettings.json` ships empty. Mail sending and anything that
reads an encrypted setting will not work until you fill in your own values, and the
connection string points at a local database.

## Notes

This is 2023 code and there are things I would do differently now. The data context,
the entities and the migrations sit in the application layer rather than in
infrastructure, which is not where a strict reading of Clean Architecture would put
them. Configuration values were encrypted in the settings file with a key stored next
to them, which protects nothing. There are no automated tests.

Today I would move the persistence into the infrastructure layer, keep secrets out of
the repository entirely, and write tests for the validation and the authorisation
before anything else.
