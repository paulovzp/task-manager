# Architecture

## Dependency direction

```text
Angular client
      |
      v
ASP.NET Core API  --->  Application  --->  Domain
      |                     ^
      v                     |
Infrastructure ------------+
      |
      v
SQL Server
```

The Domain project has no framework dependency. Application defines use cases and persistence/authentication contracts. Infrastructure implements those contracts with EF Core and Identity. API owns HTTP, JWT validation, claims, CORS, and problem details.

## Projects

- `TaskManager.Domain`: `TaskItem`, `TaskItemStatus`, and entity invariants.
- `TaskManager.Application`: CRUD command/query handlers, DTOs, exceptions, and abstractions.
- `TaskManager.Infrastructure`: Identity user, JWT creation, EF Core context, mappings, migrations, repository, and demo seed.
- `TaskManager.Api`: request validation, controllers, authorization, exception mapping, and OpenAPI.
- `task-manager-web`: routed Angular application, authentication state, JWT interceptor, reactive forms, filters, loading states, and responsive UI.

## Security

- Passwords are hashed and verified by ASP.NET Core Identity.
- JWTs use HMAC SHA-256, issuer/audience validation, expiration, and a short clock skew.
- Signing keys and the SQL password can be overridden with environment variables.
- Task queries include both task ID and authenticated owner ID.
- Missing and unowned tasks have the same `404` behavior, avoiding ownership disclosure.
- DTOs prevent entity over-posting.
- EF Core uses parameterized SQL.

For this demonstration, the Angular client keeps the access token in `sessionStorage`. This reduces persistence across browser sessions but remains exposed to successful XSS. A production evolution would use a short-lived access token in memory with a rotated refresh token in a Secure, HttpOnly, SameSite cookie.

## Persistence

`TaskManagerDbContext` extends the Identity context so users and tasks share one SQL Server database and one migration history. `TaskItemConfiguration` defines lengths, required fields, status conversion, the owner foreign key, and an owner/due-date index.

The repository applies owner filtering in the query itself. Read-only lists use `AsNoTracking`; update lookups remain tracked so a single `SaveChangesAsync` persists the change.

## Testing seams

- Domain behavior through public entity methods.
- Application behavior through command/query handlers and repository contracts.
- Persistence through the EF repository with relational SQLite.
- HTTP behavior through `WebApplicationFactory`, real Identity, JWT, and an isolated SQLite database.
- Frontend HTTP behavior through Angular services and the HTTP testing controller.
