# Taskflow Task Manager

A full-stack task manager built for the Ballast Lane .NET technical exercise. Authenticated users can register, sign in, and manage only their own tasks through a responsive Angular interface.

## User story

> As an authenticated user, I want to create and organize tasks with a title, description, status, and due date, so that I can focus on the work I need to finish.

## Stack

- .NET 10 and ASP.NET Core Web API
- Clean Architecture with Domain, Application, Infrastructure, and API projects
- ASP.NET Core Identity and JWT Bearer authentication
- Entity Framework Core 10 and SQL Server
- Angular 22, standalone components, signals, reactive forms, and Vitest
- Docker Compose with SQL Server, API, and Nginx-hosted Angular
- xUnit integration and unit tests

## Run with Docker

Requirements: Docker Desktop with Linux containers.

```powershell
Copy-Item .env.example .env
docker compose up --build
```

Open:

- Application: http://localhost:4200
- API OpenAPI document: http://localhost:8080/openapi/v1.json

The host ports can be changed in `.env` with `WEB_PORT`, `API_PORT`, and
`SQLSERVER_PORT`. Container-to-container addresses remain unchanged.

Demo credentials:

```text
Email: demo@taskmanager.local
Password: Demo1234
```

The API applies the migration and seeds the demonstration account when it starts in Docker. Change both secrets in `.env` before using the project outside a local demonstration.

Stop the application with `docker compose down`. Add `-v` only when you intentionally want to delete the SQL Server data volume.

## Run locally

Start SQL Server:

```powershell
docker compose up -d sqlserver
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
$env:SeedData = "true"
dotnet run --project src/TaskManager.Api --urls http://localhost:8080
```

In a second terminal:

```powershell
Set-Location src/task-manager-web
npm install
npm start
```

The Angular development proxy forwards `/api` to `http://localhost:8080`.

## Tests and quality checks

```powershell
dotnet test TaskManager.sln
dotnet format TaskManager.sln --verify-no-changes

Set-Location src/task-manager-web
npm test -- --watch=false
npm run build
```

The integration suite replaces SQL Server with an isolated SQLite database while exercising Identity, JWT, authorization, and the complete CRUD over HTTP.

## API

| Method | Endpoint | Access | Purpose |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Create a user and return a JWT |
| POST | `/api/auth/login` | Public | Authenticate and return a JWT |
| GET | `/api/auth/me` | Authorized | Return current user claims |
| GET | `/api/tasks` | Authorized | List the current user's tasks |
| GET | `/api/tasks/{id}` | Authorized | Read an owned task |
| POST | `/api/tasks` | Authorized | Create a task |
| PUT | `/api/tasks/{id}` | Authorized | Update an owned task |
| DELETE | `/api/tasks/{id}` | Authorized | Delete an owned task |
| GET | `/api/public/info` | Public | Demonstrate anonymous access |
| GET | `/api/secure/info` | Authorized | Demonstrate protected access |

Task ownership comes exclusively from the JWT claim. The API never trusts a user identifier supplied by the browser.

## Architecture

See [architecture.md](docs/architecture.md) for project responsibilities, dependency direction, persistence, and security decisions.

## Generative AI record

See [genai.md](docs/genai.md) for the prompt, representative generated code, validation process, corrections, and edge cases requested by the exercise.
