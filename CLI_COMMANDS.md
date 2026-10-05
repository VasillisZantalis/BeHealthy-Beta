# Useful CLI Commands

Run every command from the repo root (`BeHealthy/`) unless noted otherwise.

## Code style and formatting

```bash
# Fix everything .editorconfig defines (whitespace + style + analyzers)
dotnet format BeHealthy.sln

# Check only: change nothing, exit code 2 if anything needs fixing (good for CI)
dotnet format BeHealthy.sln --verify-no-changes
dotnet format style BeHealthy.sln --verify-no-changes   # only the .editorconfig style rules

# Fix only specific rules
dotnet format BeHealthy.sln --diagnostics IDE0011 IDE0055 --severity info

# Fix only whitespace/indentation/brace placement
dotnet format whitespace BeHealthy.sln

# Fix only code-style rules (IDExxxx), e.g. braces, var, naming
dotnet format style BeHealthy.sln --severity info

# Fix only analyzer rules (CAxxxx)
dotnet format analyzers BeHealthy.sln --severity info

# Limit to one project or some files
dotnet format src/BeHealthy.Application/BeHealthy.Application.csproj
dotnet format BeHealthy.sln --include src/BeHealthy.API/Controllers/
dotnet format BeHealthy.sln --exclude src/BeHealthy.Infrastructure/Migrations/

# Preview: write a JSON report of what would change
dotnet format BeHealthy.sln --verify-no-changes --report ./format-report
```

`--severity` picks the lowest severity to fix: `info`, `warn` (default) or `error`.

Visual Studio shortcuts: **Ctrl+K, Ctrl+E** = Code Cleanup on the file, **Ctrl+K, Ctrl+D** = format the document.

## Build and test

```bash
dotnet restore BeHealthy.sln
dotnet build BeHealthy.sln
dotnet build BeHealthy.sln -v q -nologo            # quiet: only warnings/errors + summary
dotnet build BeHealthy.sln -c Release
dotnet build BeHealthy.sln --no-incremental        # force full rebuild (re-run all analyzers)
dotnet clean BeHealthy.sln

# Count style violations without failing on them
dotnet build BeHealthy.sln -v q 2>&1 | grep -c "IDE"

# Tests
dotnet test tests/BeHealthy.Tests
dotnet test tests/BeHealthy.Tests --filter "FullyQualifiedName~DoctorServiceTests"
dotnet test tests/BeHealthy.Tests --filter "Name=CreateAsync_ReturnsSuccess"
dotnet test tests/BeHealthy.Tests --logger "console;verbosity=detailed"
dotnet test tests/BeHealthy.Tests --collect:"XPlat Code Coverage"
```

## Run the apps

```bash
dotnet run --project src/BeHealthy.API
dotnet run --project src/BeHealthy.Front
dotnet watch --project src/BeHealthy.Front          # hot reload
dotnet run --project src/BeHealthy.API --launch-profile https
```

## Solution management

```bash
dotnet sln BeHealthy.sln list
dotnet sln BeHealthy.sln add NewProject/NewProject.csproj
dotnet sln BeHealthy.sln add NewProject/NewProject.csproj --solution-folder API
dotnet sln BeHealthy.sln remove OldProject/OldProject.csproj

dotnet new classlib -n BeHealthy.Something -f net9.0
dotnet add src/BeHealthy.API reference src/BeHealthy.Something
```

Solution Items (like `.editorconfig`) can't be added via CLI. Edit the `Solution Items` block in `BeHealthy.sln` or right-click the solution in VS → Add → Existing Item.

## NuGet packages

```bash
dotnet list BeHealthy.sln package                  # what is installed
dotnet list BeHealthy.sln package --outdated       # what can be updated
dotnet list BeHealthy.sln package --vulnerable --include-transitive
dotnet add src/BeHealthy.Application package FluentValidation
dotnet remove src/BeHealthy.Application package SomePackage
```

## Entity Framework Core migrations

`dotnet-ef` is pinned in `dotnet-tools.json`. Restore it once with `dotnet tool restore`.

```bash
dotnet tool restore

dotnet ef migrations add <Name> --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API
dotnet ef migrations list       --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API
dotnet ef migrations remove     --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API   # undo last, not yet applied
dotnet ef database update       --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API
dotnet ef database update <PreviousMigrationName> --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API   # roll back

# Generate an idempotent SQL script (for production deploys)
dotnet ef migrations script --idempotent -o migrate.sql --project src/BeHealthy.Infrastructure --startup-project src/BeHealthy.API
```

> `--exclude src/BeHealthy.Infrastructure/Migrations/` with `dotnet format` keeps generated migration files untouched.

## User secrets (connection strings, keys)

```bash
dotnet user-secrets list --project src/BeHealthy.API
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=behealthy;Username=...;Password=..." --project src/BeHealthy.API
dotnet user-secrets remove "ConnectionStrings:Default" --project src/BeHealthy.API
dotnet user-secrets clear --project src/BeHealthy.API
```

## Git — keeping style changes clean

```bash
# Summary of a big reformat
git diff --stat
git diff --shortstat

# Ignore whitespace / line-ending noise when reviewing
git diff --ignore-all-space --ignore-cr-at-eol

# Undo the formatter on all .cs files (only if they had no other uncommitted work!)
git diff --name-only -- '*.cs' | xargs git checkout --

# Commit a reformat on its own so it doesn't hide real changes
git add -A && git commit -m "style: apply .editorconfig formatting"

# Hide that commit from git blame
echo <commit-sha> >> .git-blame-ignore-revs
git config blame.ignoreRevsFile .git-blame-ignore-revs
```

## CI — fail the pipeline on style violations

Add to `.github/workflows/*.yml` before the build step:

```yaml
- name: Check formatting
  run: |
    dotnet format whitespace BeHealthy.sln --verify-no-changes
    dotnet format style BeHealthy.sln --verify-no-changes
```

> Plain `dotnet format --verify-no-changes` also runs analyzers at warning level and currently fails on the existing Blazor `BL0008` warnings. Use the `whitespace` + `style` split above until those are fixed.

## .NET SDK / tooling info

```bash
dotnet --info
dotnet --list-sdks
dotnet --list-runtimes
dotnet tool list --local
dotnet tool list --global
dotnet workload list
```
