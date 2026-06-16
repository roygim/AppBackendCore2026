# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

- Build: `dotnet build`
- Run (HTTP on :5162, HTTPS on :7252): `dotnet run` or `dotnet run --launch-profile https`
- Restore: `dotnet restore`
- Swagger UI (Development only): `/swagger`; OpenAPI doc at `/openapi/v1.json`

There is no test project yet — no test commands apply.

**Do not build or run the app when finishing a task.** Make the code changes and stop — the user builds and runs the app themselves. Only run `dotnet build`/`dotnet run` if explicitly asked.

## Architecture

ASP.NET Core Web API on **.NET 10**, with `Nullable` and `ImplicitUsings` enabled. Classic 3-tier separation, one layer per top-level folder:

- `Controllers/` — HTTP endpoints (thin; delegate to services)
- `Services/` — business logic, depends on repository interfaces
- `Repositories/` — data access via **Dapper** over **MySqlConnector**
- `Interfaces/` — contracts for both services and repositories (`IUsersService`, `IUsersRepository`)
- `Models/` — domain objects (`UserObj`)
- `DTOs/` — request/response shapes that don't expose the full domain model (`UserLightDto` for reads, `CreateUserDto` for inserts, `LoginDto` for login, `LoginResultDto`, and the generic `ServiceResult<T>` envelope). DTOs follow a `*Dto` suffix and lowercase property names.
- `Data/` — `UsersDbContext`, a thin Dapper connection factory
- `Middleware/` — created but empty; reserved for future custom middleware

Dependency flow: Controller → `IUsersService` → `IUsersRepository`. The MySQL repository (`UsersDataMysql`) is the concrete `IUsersRepository` and takes `UsersDbContext` for connections. `UsersService` also depends on `ITokenService` (`TokenService`) for JWT creation. DI for all of these plus `UsersDbContext` is registered in `Program.cs` (`AddScoped`).

### Data access
- `Data/UsersDbContext.cs` reads the `csUsersMysql` connection string (from `appsettings.Development.json`) and exposes `CreateConnection()` returning a `MySqlConnection`. It throws if the connection string is missing.
- Repositories open a connection per call with `using var connection = _context.CreateConnection();` and run Dapper queries. Inserts return the new id via a trailing `SELECT LAST_INSERT_ID();` read with `ExecuteScalarAsync<int>`.
- The MySQL `users` table columns are `id, firstname, lastname, email, password`. Dapper maps these to matching property names — keep DTO/model property casing aligned with the column names (mostly lowercase) so mapping works without aliases.

### Auth (login / JWT)
- Passwords are hashed with **BCrypt.Net-Next** (`BCrypt.Net.BCrypt.HashPassword(pwd, 12)` on insert; `BCrypt.Net.BCrypt.Verify(...)` on login).
- `Services/TokenService.cs` (`ITokenService`) issues a JWT via `System.IdentityModel.Tokens.Jwt`. It reads `Jwt:SecretKey` from config, derives the HS256 signing key as `SHA256(secret)` (because the configured secret is shorter than the 256-bit minimum HS256 requires), and emits `sub` + `email` claims with a 1h expiry. NOTE: the SHA-256 derivation means tokens are **not** signature-compatible with Node's `jsonwebtoken`; use a ≥32-char raw secret and drop the hashing if cross-compat is needed.
- Service methods that can fail return `ServiceResult<T>` (`{ success, error: ErrorType?, message, data }`); the controller maps `ErrorType.UserNotFound`→404 and `InvalidPassword`→400. On success, `Login` sets an `httpOnly` `userToken` cookie.
- Token **validation** middleware / `[Authorize]` is not set up yet — tokens are only issued.

### Conventions
- **Global usings**: layer namespaces are registered once in `GlobalUsings.cs` (Interfaces, Services, Repositories, Models, Data). New files generally do not need explicit `using` for these. NOTE: `DTOs` is **not** global — files using `*Dto` types add `using AppBackendCore2026.DTOs;` per-file.
- **Namespaces** follow `AppBackendCore2026.<Layer>`. NOTE: `UsersController` still uses `UsersBackend.Controllers` — an inconsistency to fix when touching it.
- Domain model is named `UserObj` (not `User`), defined in `Models/User.cs`. Prefer returning a DTO (e.g. `UserLightDto`) from endpoints rather than `UserObj` so fields like `password` aren't exposed.

## Current Endpoints

`UsersController` (`api/users`):
- `GET /api/users/GetAll` → `List<UserLightDto>`
- `POST /api/users/AddUser` (body: `CreateUserDto`) → created `UserLightDto`
- `POST /api/users/Login` (body: `LoginDto`) → `ServiceResult<LoginResultDto>`; sets `httpOnly` `userToken` cookie. 404 if user not found, 400 if password invalid.

## Setup notes

- A working `csUsersMysql` connection string must exist in `appsettings.Development.json` and point at a reachable MySQL instance with a `users` table for the endpoints to work.
- A `Jwt:SecretKey` must exist in config (currently in `appsettings.Development.json`) for login/token issuance.
