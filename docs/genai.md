# Generative AI Usage

## Prompt used

```text
Build a .NET 10 REST API for a multi-user task management system using Clean Architecture and TDD.

Each task must contain an ID, owner ID, title, description, status, and due date. Implement registration and login with ASP.NET Core Identity and JWT. Every task endpoint must derive the owner from the authenticated token and must never accept an arbitrary owner ID from the request.

Create controller-based endpoints for list, get, create, update, and delete. Use EF Core with SQL Server in production, asynchronous repository methods, cancellation tokens, DTOs, RFC problem details, and an initial migration. Add unit tests for domain and application behavior, a relational repository test, and an API integration test that proves registration, JWT authentication, CRUD, and unauthorized access.

Do not hide business rules in controllers or EF entities. Explain security tradeoffs and do not place production secrets in source control.
```

## Skills used

Codex guidance skills were used as focused review checklists throughout the implementation. They did not replace engineering judgment or project validation:

- `tdd`: guided the red-green-refactor workflow and the choice of test seams before implementation.
- `dotnet-best-practices`: checked project structure, dependency injection, nullable reference types, configuration, exception handling, and ASP.NET Core conventions.
- `csharp-xunit`: guided test naming, isolation, assertions, and coverage of domain and application behavior.
- `csharp-async`: checked asynchronous I/O, cancellation-token propagation, and avoidance of blocking calls.
- `ef-core`: guided entity mappings, migrations, indexes, ownership predicates, tracking behavior, and relational integration tests.
- `impeccable`: guided the Angular interface review, including hierarchy, responsive behavior, accessibility, loading, empty, and error states.
- `codebase-design`: helped validate the architectural seams: application-owned interfaces, infrastructure adapters, and API-owned HTTP contracts.

The skills supplied specialized recommendations; compiler feedback, automated tests, runtime smoke tests, and manual review determined whether each recommendation was accepted.

## Representative output

The generated scaffold suggested filtering tasks after loading them. That was rejected because it could expose another user's records in memory and waste database work. The repository contract and implementation were changed so ownership is part of the database predicate:

```csharp
public Task<TaskItem?> GetByIdAsync(
    Guid id,
    Guid ownerId,
    CancellationToken cancellationToken) =>
    _context.Tasks.SingleOrDefaultAsync(
        task => task.Id == id && task.OwnerId == ownerId,
        cancellationToken);
```

The controller also derives the owner from the validated JWT rather than accepting it in a request body:

```csharp
new CreateTaskCommand(GetUserId(), request.Title, request.Description, request.DueDate)
```

## How the suggestions were validated

1. Compilation with nullable reference types, current analyzers, and warnings treated as errors.
2. Red-green TDD cycles for entity and application behavior.
3. A relational SQLite test for EF mappings and owner filtering.
4. An end-to-end in-process API test covering Identity registration, JWT, and the full CRUD.
5. Angular production build and Vitest execution.
6. Manual review of generated migration, Docker configuration, error responses, and secret handling.

## Corrections and improvements

- Replaced controller-owned business logic with command/query handlers.
- Added `TimeProvider` so future-date validation is deterministic in tests.
- Added an owner foreign key and an owner/due-date index.
- Used `AsNoTracking` for read-only lists.
- Fixed `CreatedAtAction` route generation by naming the GET route explicitly.
- Replaced SQL Server with relational SQLite only inside integration tests.
- Added cancellation tokens to I/O boundaries.
- Converted expected failures to consistent problem details.
- Used Identity password hashing instead of custom password storage.

## Edge cases considered

- Empty owner identifier.
- Missing, blank, or oversized title.
- Oversized description.
- Due date that is not in the future.
- Duplicate email and weak password errors from Identity.
- Invalid credentials.
- Missing JWT.
- Attempt to access another user's task.
- Missing task during read, update, or delete.
- Loading, empty, error, and narrow-screen states in the frontend.

AI accelerated scaffolding and review, but every output was treated as a proposal. Tests, framework documentation, compiler feedback, and security boundaries remained the sources of truth.
