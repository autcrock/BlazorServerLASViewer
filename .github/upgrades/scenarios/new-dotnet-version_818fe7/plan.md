# .NET 10.0 Upgrade Plan for BlazorServerLASViewer

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plan](#project-by-project-migration-plan)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade BlazorServerLASViewer ASP.NET Core application from **.NET Core 3.1** to **.NET 10.0 (Long Term Support)**.

### Scope
- **Projects**: 1 (BlazorServerLASViewer)
- **Total LOC**: 574
- **NuGet Packages**: 2 (1 compatible, 1 incompatible)
- **Current State**: .NET Core 3.1
- **Target State**: .NET 10.0

### Key Findings from Assessment

| Item | Status |
|------|--------|
| Project Target Framework | ❌ Requires update from `netcoreapp3.1` to `net10.0` |
| Incompatible Packages | ⚠️ `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` v1.10.9 must be removed |
| API Breaking Changes | 🟡 1 behavioral change in `UseExceptionHandler()` API (potential, requires testing) |
| Compatible Packages | ✅ `BlazorInputFile` v0.2.0 (no update needed) |

### Solution Complexity Assessment

**Classification: SIMPLE**
- Single project (< 5 projects threshold)
- No inter-project dependencies
- Small codebase (574 LOC)
- Clear upgrade path with minimal issues
- Only 1 incompatible package to remove
- Only 1 API behavioral change to address

### Selected Strategy

**All-At-Once Strategy** — Single atomic upgrade operation for the entire solution.

**Rationale**:
- ✅ Only 1 project to upgrade
- ✅ No project-to-project dependencies
- ✅ Simple dependency structure (2 NuGet packages)
- ✅ Clear upgrade path with all target versions identified
- ✅ Small codebase suitable for coordinated upgrade
- ✅ Enables fast completion with single testing phase

### Migration Approach

1. **Atomic Operation**: All project file and package updates applied simultaneously
2. **Single Test Phase**: Comprehensive validation after upgrade completes
3. **No Intermediate States**: One coordinated upgrade, no phased rollout needed
4. **Timeline**: Minimal, single pass with build verification and testing

---

---

## Migration Strategy

### Approach: All-At-Once Upgrade

This solution uses the **All-At-Once strategy**, which performs a single atomic upgrade operation on the entire solution simultaneously. This is ideal for simple, single-project solutions with clear dependencies.

#### Why All-At-Once for This Solution?

- **Single Project**: No coordination between multiple projects required
- **No Project Dependencies**: Project stands alone with no inter-project references
- **Simple Dependency Graph**: Only 2 NuGet packages (1 to remove, 1 already compatible)
- **Clear Issues**: All required changes are well-defined (framework, packages, 1 API change)
- **Small Codebase**: 574 LOC allows safe comprehensive testing in single pass

#### Implementation Approach

The upgrade will be executed as a **unified operation**:

1. **Phase 1: Atomic Upgrade** (Single coordinated operation)
   - Update project file `TargetFramework` to `net10.0`
   - Remove incompatible `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package
   - Restore dependencies with `dotnet restore`
   - Build solution and identify compilation errors
   - Fix all breaking changes (API behavioral change in `Startup.cs`)
   - Rebuild to verify success

2. **Phase 2: Test Validation** (Single comprehensive test phase)
   - Execute all unit tests
   - Address any test failures
   - Validate functionality end-to-end

#### No Intermediate States

Because this is a single-project solution, there are no intermediate build/test states. The entire upgrade completes in one atomic operation, then proceeds to comprehensive testing.

---

## Detailed Dependency Analysis

### Project Graph

```
BlazorServerLASViewer.csproj (net10.0 target)
  ├─ BlazorInputFile 0.2.0 ✅ (compatible)
  └─ Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.10.9 ❌ (remove)
```

### Dependency Structure

- **Total Projects**: 1
- **Project Dependencies**: 0 (no inter-project references)
- **NuGet Dependencies**: 2
  - **Compatible**: 1 package
  - **Incompatible**: 1 package (marked for removal)

### Migration Order

Because there is only 1 project with no dependencies, it upgrades as a single unit:

**Migration Phase**: All Projects (Atomic)
- `BlazorServerLASViewer.csproj` (currently `netcoreapp3.1` → target `net10.0`)

### Critical Path

The entire solution is the critical path. All upgrades must complete before testing can proceed.

---

---

## Project-by-Project Migration Plan

### BlazorServerLASViewer.csproj

#### Current State
- **Project Type**: ASP.NET Core (Blazor Server)
- **Current Target Framework**: `netcoreapp3.1`
- **SDK-Style Project**: Yes
- **Lines of Code**: 574
- **Files**: 52 files (2 with issues)
- **Risk Level**: 🟢 Low
- **Complexity**: 🟢 Low

**Current Project Dependencies**:
- `BlazorInputFile` v0.2.0 ✅
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` v1.10.9 ❌

#### Target State
- **Target Framework**: `net10.0`
- **NuGet Packages**: Only `BlazorInputFile` v0.2.0 (compatible, no update needed)

#### Migration Steps

**Step 1: Update Project File (BlazorServerLASViewer.csproj)**

Modify the project file to update the target framework:

```xml
<!-- BEFORE -->
<TargetFramework>netcoreapp3.1</TargetFramework>

<!-- AFTER -->
<TargetFramework>net10.0</TargetFramework>
```

**Step 2: Remove Incompatible Package**

Remove the `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package reference from the project file:

```xml
<!-- REMOVE THIS PACKAGE REFERENCE -->
<ItemGroup>
  <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.10.9" />
</ItemGroup>
```

**Why**: This package has no compatible version for .NET 10.0. Docker container tooling is built into .NET 10.0 SDK, so this package is no longer needed.

**Step 3: Restore Dependencies**

After project file changes, restore NuGet packages:
```bash
dotnet restore BlazorServerLASViewer.csproj
```

**Step 4: Compile and Identify Breaking Changes**

Build the project to identify any compilation errors from API changes:
```bash
dotnet build BlazorServerLASViewer.csproj
```

Expected issue: API behavioral change in `Startup.cs` line 38 (see §Breaking Changes Catalog).

**Step 5: Fix Breaking Changes**

Address the `UseExceptionHandler()` API change in `Startup.cs` (detailed in §Breaking Changes Catalog).

**Step 6: Rebuild and Verify**

Rebuild the project to verify all changes applied correctly:
```bash
dotnet build BlazorServerLASViewer.csproj
```

**Expected Outcome**: Project builds with 0 errors and 0 warnings.

#### Package Updates Summary

| Package | Current | Action | Reason |
|---------|---------|--------|--------|
| BlazorInputFile | 0.2.0 | Keep (no update) | Compatible with .NET 10.0 |
| Microsoft.VisualStudio.Azure.Containers.Tools.Targets | 1.10.9 | Remove | No compatible version for .NET 10.0; functionality built into SDK |

#### Files Requiring Code Changes

| File | Issue | Line(s) | Change Required |
|------|-------|---------|-----------------|
| `Startup.cs` | API behavioral change in `UseExceptionHandler()` | 38 | See §Breaking Changes Catalog |

#### Validation Checklist

After migration, verify:
- [ ] Project file has `<TargetFramework>net10.0</TargetFramework>`
- [ ] No `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package reference
- [ ] `dotnet restore` completes without errors
- [ ] `dotnet build` produces 0 errors
- [ ] All API changes addressed (Startup.cs UseExceptionHandler)
- [ ] Unit tests pass
- [ ] Application starts successfully
- [ ] No runtime errors or warnings

---

---

## Package Update Reference

### Summary
- **Total Packages**: 2
- **Packages to Keep**: 1 (compatible with .NET 10.0)
- **Packages to Remove**: 1 (no compatible version)
- **Packages to Update**: 0 (no updates required)

### Detailed Package Matrix

| Package | Current Version | Action | Target Version | Affected Projects | Reason |
|---------|-----------------|--------|-----------------|-------------------|--------|
| BlazorInputFile | 0.2.0 | Keep | 0.2.0 | BlazorServerLASViewer.csproj | ✅ Already compatible with .NET 10.0 |
| Microsoft.VisualStudio.Azure.Containers.Tools.Targets | 1.10.9 | **Remove** | — | BlazorServerLASViewer.csproj | ⚠️ No compatible version for .NET 10.0; Docker tooling built into SDK |

### Package Notes

**BlazorInputFile v0.2.0**
- Status: ✅ Compatible
- No update needed
- Will continue to work with .NET 10.0

**Microsoft.VisualStudio.Azure.Containers.Tools.Targets v1.10.9**
- Status: ⚠️ Incompatible
- Action: Remove from project file
- Reason: .NET 10.0 includes built-in Docker support; this external package is no longer necessary
- Migration Path: Remove the NuGet reference; Docker support is available through .NET 10.0 SDK

---

## Breaking Changes Catalog

### Overview
The upgrade from .NET Core 3.1 to .NET 10.0 introduces **1 known behavioral change** affecting the codebase. This represents a low-risk API behavior change that requires testing validation.

### Behavioral Changes

#### 1. ExceptionHandler API - `UseExceptionHandler()` Method

**Location**: `Startup.cs`, line 38

**Current Code**:
```csharp
app.UseExceptionHandler("/Error");
```

**Issue**: In .NET 10.0, the `UseExceptionHandler(string)` overload has changed its behavior.

**Breaking Change Type**: Behavioral Change (Low Risk)
- The method signature remains the same
- Code will compile without errors
- Behavior at runtime may differ from .NET Core 3.1

**What Changed**:
- In .NET Core 3.1: `UseExceptionHandler("/Error")` routes exceptions to the specified path
- In .NET 10.0: Exception handling pipeline has been improved; the error handling flow may route requests differently

**Mitigation**:
- ✅ **Recommended Approach**: Test the application thoroughly after upgrade to verify exception handling works as expected
- ✅ **Alternative** (if issues found): Explicitly configure exception handling using `UseExceptionHandler(builder => { /* custom logic */ })`

**Validation Required**:
1. Test normal exception scenarios (e.g., unhandled exceptions, 404s)
2. Verify error page displays correctly
3. Confirm logging of exceptions still works
4. Test error page styling/assets load properly

**Reference**: [ASP.NET Core Breaking Changes Documentation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnetcore)

### Framework-Level Breaking Changes

The following breaking changes from .NET Core 3.1 to .NET 10.0 should be monitored:

**Configuration Changes**:
- ASP.NET Core middleware registration patterns remain compatible
- Service configuration in `ConfigureServices()` compatible

**Deprecations**:
- No APIs used in this codebase are deprecated

**Removed APIs**:
- No removed APIs detected in assessment

### Other Behavioral Considerations

**Nullable Reference Types**:
- .NET 5.0+ enables nullable reference types by default
- Code may generate new warnings (non-breaking, compile succeeds)
- Review warnings and add `#nullable` directives if needed

**Build System**:
- SDK-style project format (already in use) fully compatible
- No project file restructuring needed

---

---

## Risk Management

### Risk Assessment Summary

| Risk Category | Level | Projects Affected | Mitigation |
|---------------|-------|-------------------|-----------|
| **Framework Update** | 🟢 Low | BlazorServerLASViewer | Well-documented upgrade path; many breaking changes known in advance |
| **Package Removal** | 🟢 Low | BlazorServerLASViewer | Docker tooling now built into .NET 10.0 SDK; safe to remove |
| **API Behavioral Change** | 🟢 Low | BlazorServerLASViewer | Single known change; requires testing validation only |
| **Test Coverage** | 🟢 Low | BlazorServerLASViewer | Small codebase (574 LOC); easy to thoroughly test |

### High-Risk Items
**None identified.** This upgrade is low-risk due to:
- Single small project
- No project dependencies
- Only 1 package to remove (safe removal)
- Only 1 behavioral change (requires testing)
- No security vulnerabilities

### Mitigation Strategies

#### For Package Removal

**Risk**: Removing `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` breaks Docker support

**Mitigation**:
- ✅ Docker tooling is built into .NET 10.0 SDK
- ✅ No additional configuration needed
- ✅ Container support available through `dotnet publish --os linux --arch x64`
- ✅ If using Docker Compose, no changes needed—just use updated .NET 10.0 base images

**Contingency**: If Docker support is critical:
- Verify Docker builds succeed after upgrade
- Fall back to explicit Docker configuration if needed
- Review [.NET Docker images documentation](https://hub.docker.com/_/microsoft-dotnet)

#### For API Behavioral Change

**Risk**: `UseExceptionHandler()` behavior changes may affect error handling

**Mitigation**:
- ✅ Thoroughly test exception scenarios (unhandled exceptions, 404s, etc.)
- ✅ Verify error page displays correctly
- ✅ Monitor application logs during testing

**Contingency**: If exception handling breaks:
- Implement explicit exception handler: `UseExceptionHandler(builder => { /* logic */ })`
- Review ASP.NET Core error handling documentation
- Consider implementing custom middleware for exception handling

### Security Considerations

**Current Status**: ✅ No security vulnerabilities detected in assessment

**Post-Upgrade**: 
- .NET 10.0 includes latest security patches
- Regular updates recommended per Microsoft's support timeline
- No additional security hardening required for this upgrade

### Rollback Plan

**If critical issues are discovered**:

1. **Immediate Rollback**:
   - Revert project file changes (restore `netcoreapp3.1` TargetFramework)
   - Restore `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package reference
   - Run `dotnet restore` and `dotnet build`

2. **Investigation**:
   - Document what failed
   - Search for workarounds in Microsoft documentation
   - Consider incremental upgrade (e.g., to .NET 6.0 or 8.0 first)

3. **Escalation**:
   - If unresolvable, delay upgrade until additional testing or patches available

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

#### Level 1: Compilation Validation
**After project file updates and before execution**:
- [ ] `dotnet restore` completes successfully
- [ ] `dotnet build` produces 0 errors
- [ ] `dotnet build` produces 0 warnings (or documented acceptable warnings)

#### Level 2: Unit Testing
**After successful compilation**:
- [ ] Execute all unit tests: `dotnet test`
- [ ] All tests pass (0 failures)
- [ ] Verify test coverage not degraded

#### Level 3: Integration Testing
**Functional verification**:
- [ ] Application starts successfully: `dotnet run`
- [ ] All pages load without errors
- [ ] All features work as expected
- [ ] Database connections (if applicable) work correctly

#### Level 4: Exception Handling Testing
**Specific to known breaking change**:
- [ ] Trigger unhandled exception; verify error page displays
- [ ] Verify error logging is working
- [ ] Verify error page styling and assets load correctly
- [ ] Test 404 error handling (not found route)
- [ ] Test 500 error handling (server errors)

#### Level 5: Runtime Validation
**Post-deployment testing**:
- [ ] Monitor application logs for warnings/errors
- [ ] Verify performance is acceptable (no slowdowns)
- [ ] Confirm Docker builds succeed (if using containers)
- [ ] Smoke test critical business functions

### Testing Checklist

**Pre-Upgrade**:
- [ ] Current tests pass on .NET Core 3.1
- [ ] All test projects identified
- [ ] Test data/fixtures available

**Post-Upgrade**:
- [ ] All compilation errors resolved
- [ ] All unit tests pass
- [ ] All integration tests pass
- [ ] Exception handling works correctly
- [ ] No runtime errors in logs
- [ ] Performance acceptable

---

---

## Complexity & Effort Assessment

### Solution-Level Complexity

| Metric | Value | Assessment |
|--------|-------|------------|
| **Total Projects** | 1 | Simple (≤5) |
| **Total LOC** | 574 | Small (< 1k) |
| **Dependency Depth** | 0 | Minimal (leaf project) |
| **External Package Dependencies** | 2 | Low |
| **Known Breaking Changes** | 1 | Low |
| **Estimated Complexity** | 🟢 **Low** | Single-pass upgrade, minimal testing surface |

### Per-Project Complexity Breakdown

#### BlazorServerLASViewer.csproj

| Dimension | Rating | Notes |
|-----------|--------|-------|
| **Code Complexity** | 🟢 Low | 574 LOC, small Blazor app |
| **Dependency Complexity** | 🟢 Low | 2 NuGet packages; no inter-project dependencies |
| **Breaking Changes** | 🟢 Low | 1 behavioral change in exception handling |
| **Risk Level** | 🟢 Low | No security vulnerabilities; safe package removal |
| **Testing Effort** | 🟢 Low | Small codebase; easy to comprehensively test |
| **Overall Difficulty** | 🟢 **Low** | Straightforward single-project upgrade |

### Relative Effort Estimate

| Phase | Effort | Details |
|-------|--------|---------|
| **Atomic Upgrade** | 🟢 Minimal | Update 1 project file, remove 1 package, fix 1 API change |
| **Testing** | 🟢 Minimal | Small codebase allows thorough testing in single pass |
| **Validation** | 🟢 Minimal | Build verification, unit tests, manual smoke testing |
| **Total** | 🟢 **Minimal** | Complete upgrade expected to be straightforward |

### Resource Requirements

- **Skill Level**: Intermediate .NET developer
- **Dependencies**: .NET 10.0 SDK installed locally
- **Parallel Capacity**: N/A (single project, no parallelization needed)
- **Time Sensitivity**: Low (straightforward upgrade path)

---

## Source Control Strategy

### Branching Strategy

**Current Branch**: `upgrade-to-NET10` (created at workflow initialization)

**Strategy**:
- All upgrade changes committed to `upgrade-to-NET10` branch
- Single atomic commit for entire upgrade (recommended for simple solutions)
- Merge to `master` via pull request after validation

### Commit Strategy

#### Single Atomic Commit (Recommended)

**Why**: For simple, single-project upgrades, a single atomic commit provides clear traceability and makes rollback simple if needed.

**Commit Message**:
```
Upgrade BlazorServerLASViewer to .NET 10.0

- Update project target framework: netcoreapp3.1 → net10.0
- Remove incompatible package: Microsoft.VisualStudio.Azure.Containers.Tools.Targets
- Fix API behavioral change in Startup.cs: UseExceptionHandler() 
- All unit tests passing
- Build succeeds with 0 errors and 0 warnings

Resolves: .NET 3.1 → .NET 10.0 upgrade
```

**Scope**: Single commit includes:
- Project file updates (TargetFramework, PackageReference changes)
- Code changes (Startup.cs exception handling fixes)
- All related changes for upgrade completion

#### Alternative: Logical Commits (if preferred)

If you prefer multiple commits for clarity:

1. **Commit 1**: "Update target framework and package references"
2. **Commit 2**: "Fix API breaking changes in Startup.cs"

### Pull Request Process

**Before Merge**:
- [ ] All code changes reviewed
- [ ] All tests pass in CI/CD pipeline
- [ ] No merge conflicts
- [ ] Build succeeds on target branch

**PR Description Template**:
```markdown
## .NET 10.0 Upgrade

**Summary**: Upgrade BlazorServerLASViewer from .NET Core 3.1 to .NET 10.0

**Changes**:
- Updated TargetFramework property in project file
- Removed incompatible package: Microsoft.VisualStudio.Azure.Containers.Tools.Targets
- Fixed API behavioral change in Startup.cs

**Testing**:
- ✅ All unit tests pass
- ✅ Application builds without errors or warnings
- ✅ Manual validation complete

**Risk Assessment**: Low risk (single project, straightforward upgrade)

**Related Issues**: [Link to issue if applicable]
```

### Post-Merge Strategy

After merge to `master`:
- Deploy to appropriate environment (dev/staging/prod)
- Monitor logs for issues
- Verify in production environment

---

---

## Success Criteria

### Technical Success Criteria

The .NET 10.0 upgrade is **complete and successful** when:

#### Framework Migration
- [ ] **BlazorServerLASViewer.csproj** TargetFramework property = `net10.0`
- [ ] Project file uses SDK-style format (already in place)

#### Compilation
- [ ] `dotnet restore` completes without errors
- [ ] `dotnet build BlazorServerLASViewer.csproj` produces **0 compilation errors**
- [ ] `dotnet build` produces **0 warnings** (or documented, acceptable warnings)

#### Package Management
- [ ] `BlazorInputFile` v0.2.0 referenced and resolves correctly
- [ ] `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` completely removed
- [ ] No deprecated package warnings
- [ ] No package dependency conflicts

#### Code Changes
- [ ] `Startup.cs` line 38: `UseExceptionHandler()` verified working (tested via exception scenarios)
- [ ] All API changes from .NET Core 3.1 → .NET 10.0 addressed
- [ ] No obsolete API usage remaining

#### Testing
- [ ] All unit tests pass: `dotnet test BlazorServerLASViewer.sln` returns 0 failures
- [ ] No new test failures introduced
- [ ] Test code updated if needed for .NET 10.0 compatibility
- [ ] Exception handling scenarios validated (manual testing):
  - [ ] Unhandled exception → error page displays
  - [ ] 404 error → error page displays correctly
  - [ ] 500 error → error page displays correctly
  - [ ] Error logging functional

#### Runtime Validation
- [ ] Application runs without errors: `dotnet run BlazorServerLASViewer.csproj`
- [ ] All web pages load without 404 or 500 errors
- [ ] No runtime warnings in application logs
- [ ] Database connections work (if applicable)
- [ ] Blazor interactivity functional
- [ ] File uploads work (`BlazorInputFile` component functional)

#### Docker/Container Support (if applicable)
- [ ] Docker builds succeed with updated .NET 10.0 base images
- [ ] Container runs without errors
- [ ] Container-based exception handling verified

### Process Success Criteria

- [ ] All changes committed to `upgrade-to-NET10` branch
- [ ] Commit message clearly describes changes
- [ ] All changes code-reviewed
- [ ] PR created and reviewed before merge
- [ ] Merge conflict resolution completed
- [ ] Deployment plan documented

### Quality Success Criteria

- [ ] Code quality not degraded (no new code smells)
- [ ] Test coverage maintained or improved
- [ ] Documentation updated (if applicable)
- [ ] Performance acceptable (no noticeable slowdowns)
- [ ] Security not compromised (no new vulnerabilities)

### Sign-Off Checklist

Before declaring upgrade complete:

**Developer Sign-Off**:
- [ ] All code changes reviewed and tested locally
- [ ] All tests pass
- [ ] No breaking changes in dependent systems
- [ ] Ready for staging/production

**QA Sign-Off** (if applicable):
- [ ] Functional testing complete
- [ ] All known issues resolved or documented
- [ ] Performance acceptable
- [ ] Security review passed

**Operations Sign-Off** (if applicable):
- [ ] Deployment process documented
- [ ] Rollback plan tested (optional but recommended)
- [ ] Monitoring/alerting configured
- [ ] Ready for production deployment

---

## Appendix: Quick Reference

### Key Commands

```bash
# Build project
dotnet build BlazorServerLASViewer.csproj

# Restore packages
dotnet restore

# Run tests
dotnet test BlazorServerLASViewer.sln

# Run application
dotnet run

# Check for deprecated/obsolete APIs
dotnet build --no-restore /p:TreatWarningsAsErrors=true
```

### Files to Update

- `BlazorServerLASViewer.csproj` — Project file (TargetFramework, PackageReferences)
- `Startup.cs` (line 38) — Exception handler implementation

### Important Versions

| Component | Version |
|-----------|---------|
| Target Framework | .NET 10.0 |
| Keep Package | BlazorInputFile 0.2.0 |
| Remove Package | Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.10.9 |

---

**Plan Complete** ✅

This plan provides a clear roadmap for upgrading BlazorServerLASViewer from .NET Core 3.1 to .NET 10.0. The All-At-Once strategy is ideal for this simple, single-project solution and enables fast, safe completion with comprehensive testing.

