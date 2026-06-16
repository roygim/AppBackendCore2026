# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

- Build: `dotnet build`
- Run (HTTP on :5162, HTTPS on :7252): `dotnet run` or `dotnet run --launch-profile https`
- Restore: `dotnet restore`
- Swagger UI (Development only): `/swagger`; OpenAPI doc at `/openapi/v1.json`

There is no test project yet — no test commands apply.

## Architecture

ASP.NET Core Web API on **.NET 10**, with `Nullable` and `ImplicitUsings` enabled. Classic 3-tier separation, one layer per top-level folder:

- `Controllers/` — HTTP endpoints (thin; delegate to services)
- `Services/` — business logic, depends on repository interfaces
- `Repositories/` — data access (intended target: MySQL via Dapper)
- `Interfaces/` — contracts for both services and repositories (`IUsersService`, `IUsersRepository`)
- `Models/` — domain objects (`UserObj`)
- `DTOs/`, `Data/`, `Middleware/` — created but empty; reserved for future DTOs, a DbContext, and custom middleware

Dependency flow: Controller → `IUsersService` → `IUsersRepository`. The MySQL repository (`UsersDataMysql`) is the concrete `IUsersRepository`.

### Conventions
- **Global usings**: layer namespaces are registered once in `GlobalUsings.cs` (Interfaces, Services, Repositories, Models). New files generally do not need explicit `using` for these; add new layer namespaces there rather than per-file.
- **Namespaces** follow `AppBackendCore2026.<Layer>`. NOTE: `UsersController` currently uses `UsersBackend.Controllers` — an inconsistency to fix when touching it.
- Domain model is named `UserObj` (not `User`), defined in `Models/User.cs`.

## Current State / Gotchas

This project is scaffolded but not yet functional end-to-end. Before relying on the Users endpoint:

- **DI is not registered.** `Program.cs` does not call `AddScoped`/`AddSingleton` for `IUsersService`/`IUsersRepository`, so the controller will fail to resolve at runtime. Register them in `Program.cs`.
- **Repository is a stub.** `UsersDataMysql.GetAll()` returns `null`; the Dapper implementation is commented out and there is no `DapperDBContext`, connection string, or DB config yet.
- **`UserObj` is minimal** — only an `Id` property.
- No connection string exists in `appsettings.json` / `appsettings.Development.json`.
