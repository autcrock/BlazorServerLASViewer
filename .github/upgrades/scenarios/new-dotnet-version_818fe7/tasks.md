# BlazorServerLASViewer .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of upgrading `BlazorServerLASViewer` from .NET Core 3.1 to .NET 10.0. Tasks cover prerequisites verification, project and package updates with compilation fixes, test execution, and the final commit.

**Progress**: 3/4 tasks complete (75%) ![75%](https://progress-bar.xyz/75)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-02-10 03:08)*
**References**: Plan §Prerequisites, Plan §Implementation Timeline Phase 0

- [✓] (1) Verify required `.NET 10.0` SDK is installed per Plan §Prerequisites (e.g., `dotnet --list-sdks` or `dotnet --version`)
- [✓] (2) Runtime/SDK version meets minimum requirements (`net10.0`) (**Verify**)
- [✓] (3) Check `global.json` (if present) for SDK pinning and update or document compatibility per Plan §Prerequisites
- [✓] (4) Configuration files and required tools (CLI, optional container tooling) compatible with target runtime (**Verify**)

### [✓] TASK-002: Update project target framework, remove incompatible package, restore and fix compilation *(Completed: 2026-02-10 03:13)*
**References**: Plan §Project-by-Project Migration Plan, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Update `BlazorServerLASViewer.csproj` TargetFramework to `net10.0` per Plan §Project-by-Project Migration Plan
- [✓] (2) Remove `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package reference from `BlazorServerLASViewer.csproj` per Plan §Package Update Reference
- [✓] (3) Restore dependencies: run `dotnet restore BlazorServerLASViewer.csproj` and ensure restore completes successfully (**Verify**)
- [✓] (4) Build project to identify compilation errors: run `dotnet build BlazorServerLASViewer.csproj` (identify issues per Plan §Breaking Changes Catalog)
- [✓] (5) Fix all compilation errors found (reference Plan §Breaking Changes Catalog, e.g., `Startup.cs` `UseExceptionHandler()` behavioral change)
- [✓] (6) Rebuild to verify fixes: run `dotnet build BlazorServerLASViewer.csproj` and ensure solution/project builds with 0 errors (**Verify**)

### [✓] TASK-003: Run full test suite and validate upgrade *(Completed: 2026-02-10 13:15)*
**References**: Plan §Testing & Validation Strategy, Plan §Success Criteria

- [✓] (1) Run unit and test projects: `dotnet test BlazorServerLASViewer.sln` per Plan §Testing & Validation Strategy
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for common fixes)
- [✓] (3) Re-run `dotnet test BlazorServerLASViewer.sln` after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

### [ ] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all remaining changes with message: "TASK-004: Complete upgrade to .NET 10.0"




