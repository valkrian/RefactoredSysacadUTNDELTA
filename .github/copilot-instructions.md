# Copilot instructions — RefactoredSysacadUTNDELTA

Purpose: help an AI coding agent get productive quickly in this mono-repo by summarizing architecture, workflows, conventions and concrete examples.

## Big picture
- Modular-monolith, Clean Architecture layers (see documentation/adr/0001-modular-monolith-clean-architecture.md):
  - Autogestion.Domain (entities, enums, pure domain rules)
  - Autogestion.Application (DTOs, service interfaces)
  - Autogestion.Infrastructure (EF Core DbContext, migrations, service implementations, seed)
  - Autogestion.Api (Minimal API endpoints, DI wiring)
  - frontend (Angular app)

## Key patterns and "why"
- DTOs live in `Autogestion.Application/DTOs` (example: `PlanDto.cs`) to avoid exposing EF entities from the API.
- Service interfaces live in `Autogestion.Application/Interfaces` and implementations in `Autogestion.Infrastructure/Services` (example: `IPlanService` / `PlanService`).
- Database access and mapping to DTOs happens in Infrastructure (use EF Core `Include`/`ThenInclude`, LINQ projections).
- Minimal APIs: endpoints are defined in `Autogestion.Api/Program.cs` and receive services via DI (example: `GET /students/me/plan`).
- Database migrations are kept in `Autogestion.Infrastructure/Migrations` and applied against the Infrastructure project.
- Seeds run only in Development when `SeedData` is true (`Autogestion.Infrastructure/data/Seed/SeedData.cs`).

## Developer workflows (concrete commands)
- Build entire solution: `dotnet build backend/Autogestion.sln` (workspace tasks available: build/publish/watch)
- Run API locally: `cd backend/Autogestion.Api && dotnet run` (or `dotnet watch run --project backend/Autogestion.Api/Autogestion.Api.csproj`)
  - Swagger available in Development at `http://localhost:5018/swagger` (see `Properties/launchSettings.json`).
- DB (Postgres) for local dev: `docker compose up -d` (root `docker-compose.yml` defines `postgres`).
- Apply migrations:
  - `dotnet ef database update --project ../Autogestion.Infrastructure/Autogestion.Infrastructure.csproj --startup-project ./Autogestion.Api.csproj`
- Create migration:
  - `dotnet ef migrations add <Name> --project ../Autogestion.Infrastructure/Autogestion.Infrastructure.csproj --startup-project ./Autogestion.Api.csproj`
- Tests: `dotnet test backend/Autogestion.Domain.Tests/Autogestion.Domain.Tests.csproj` (xUnit, domain-level tests — no DB required)
- Frontend: `cd frontend && npm install && npm start` (Angular dev server runs on :4200)

## Project-specific notes / gotchas
- `appsettings.Development.json` is ignored by git; prefer env vars (`ConnectionStrings__DefaultConnection`, `SeedData`) or create local `appsettings.Development.json` per docs.
- Seed creates a demo student (id=1) and plan data. Demo credentials in seed: `demo@utn.local` / `PasswordHash:"demo"` — useful for quick local UIs.
- Keep domain pure (no EF core references). Domain rules and tests are the source of truth for business logic (see `Autogestion.Domain.Services.CorrelativityRules` and `Autogestion.Domain.Tests`).
- When adding new UI "buttons" follow the documented pattern (documentation/docs/README.dev.es.md): DTO → I*Service → Infrastructure implementation → API endpoint → Frontend model/service/component.

## Files to open first when making changes
- `backend/Autogestion.Api/Program.cs` (endpoints, DI, seeding)
- `backend/Autogestion.Infrastructure/data/ApplicationDbContext.cs`
- `backend/Autogestion.Infrastructure/data/Seed/SeedData.cs`
- `backend/Autogestion.Application/DTOs/` and `.../Interfaces/`
- `backend/Autogestion.Infrastructure/Services/` (example: `PlanService.cs`)
- `documentation/docs/README.dev.es.md` (developer-run & replication steps)

## PR guidance for agents
- Keep changes small and focused. If schema changes are required, add a migration and update docs.
- Add/adjust unit tests (prefer `Autogestion.Domain.Tests` for domain logic; add integration tests only if necessary).
- Update `documentation/docs/README.dev.es.md` when behavior/flows change.

---
If anything above is unclear or you want extra examples (e.g., a sample migration + test), tell me what to expand. Thank you! Please review for missing details or local developer steps.