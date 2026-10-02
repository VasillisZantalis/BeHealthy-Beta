# Code Style Rules

How code style is configured and enforced in BeHealthy, plus a catalog of extra rules that can be adopted later.

## How it works

| File | Purpose |
|------|---------|
| `.editorconfig` (repo root) | Defines the rules. `root = true` stops lookup at this folder, so no parent config leaks in. Applies to every project in the repo. |
| `Directory.Build.props` (repo root) | Imported automatically by every `.csproj`. Sets `EnforceCodeStyleInBuild=true` so `IDExxxx` rules run during `dotnet build`, not just in the editor. |

Both are listed under **Solution Items** in `BeHealthy.sln`.

### Severity levels

Set per rule with `dotnet_diagnostic.<ID>.severity = <level>`:

| Level | Effect |
|-------|--------|
| `error` | Build fails |
| `warning` | Build succeeds, shows warning |
| `suggestion` | IDE only (dotted underline), not reported in build output |
| `silent` | Available as a quick fix, never shown |
| `none` | Disabled |

> The `option = value:severity` shorthand (e.g. `csharp_prefer_braces = true:error`) is **only honoured by the IDE**. For the build to enforce a rule you need the `dotnet_diagnostic.<ID>.severity` line.

---

## Rules currently enforced

| Rule | Setting | Severity | Meaning |
|------|---------|----------|---------|
| IDE0011 | `csharp_prefer_braces = true` | error | Every `if`/`else`/`for`/`foreach`/`while`/`using` body needs `{ }` |
| IDE0055 | `csharp_new_line_before_open_brace = all` | error | Opening brace on its own line (Allman style) |
| IDE0055 | `csharp_preserve_single_line_blocks = true` | error | Short blocks like `{ get; set; }` may stay on one line |
| IDE0055 | `csharp_preserve_single_line_statements = false` | error | No multiple statements on one line |
| CA2016 | — | warning | Forward an available `CancellationToken` to async calls |

---

## Rules you can adopt later

Copy the snippet into the matching section of `.editorconfig`, then run `dotnet format BeHealthy.sln` to fix existing code before building (see `CLI_COMMANDS.md`).

### General file settings (all file types)

Put these in a `[*]` section **above** `[*.cs]`:

```ini
[*]
charset = utf-8
end_of_line = crlf
indent_style = space
indent_size = 4
insert_final_newline = true
trim_trailing_whitespace = true

[*.{json,yml,yaml,csproj,props,targets,xml}]
indent_size = 2

[*.md]
trim_trailing_whitespace = false   # trailing double space = line break in Markdown
```

> `end_of_line` should agree with `.gitattributes` (`* text=auto`), otherwise Git and the editor fight over line endings.

### Namespaces and usings

The codebase already uses file-scoped namespaces.

```ini
csharp_style_namespace_declarations = file_scoped
dotnet_diagnostic.IDE0161.severity = error            # require file-scoped namespace

csharp_using_directive_placement = outside_namespace
dotnet_diagnostic.IDE0065.severity = warning

dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false
```

**Remove unused usings (IDE0005)** needs XML doc generation on to work in the build. Add to `Directory.Build.props`:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);CS1591</NoWarn>   <!-- don't demand XML comments on every public member -->
```

and to `.editorconfig`:

```ini
dotnet_diagnostic.IDE0005.severity = warning
```

### `var` usage

```ini
csharp_style_var_for_built_in_types = true
csharp_style_var_when_type_is_apparent = true
csharp_style_var_elsewhere = true
dotnet_diagnostic.IDE0007.severity = warning   # use 'var'
# or the opposite: IDE0008 = use explicit type
```

### Naming conventions

The codebase uses `_camelCase` for private fields and `I` prefix for interfaces.

```ini
# Symbols
dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private
dotnet_naming_symbols.interfaces.applicable_kinds = interface
dotnet_naming_symbols.async_methods.applicable_kinds = method
dotnet_naming_symbols.async_methods.required_modifiers = async
dotnet_naming_symbols.constants.applicable_kinds = field
dotnet_naming_symbols.constants.required_modifiers = const

# Styles
dotnet_naming_style.underscore_camel.capitalization = camel_case
dotnet_naming_style.underscore_camel.required_prefix = _
dotnet_naming_style.i_prefix.capitalization = pascal_case
dotnet_naming_style.i_prefix.required_prefix = I
dotnet_naming_style.async_suffix.capitalization = pascal_case
dotnet_naming_style.async_suffix.required_suffix = Async
dotnet_naming_style.pascal.capitalization = pascal_case

# Rules (more specific first)
dotnet_naming_rule.constants_pascal.symbols = constants
dotnet_naming_rule.constants_pascal.style = pascal
dotnet_naming_rule.constants_pascal.severity = warning

dotnet_naming_rule.private_fields_underscore.symbols = private_fields
dotnet_naming_rule.private_fields_underscore.style = underscore_camel
dotnet_naming_rule.private_fields_underscore.severity = warning

dotnet_naming_rule.interfaces_i.symbols = interfaces
dotnet_naming_rule.interfaces_i.style = i_prefix
dotnet_naming_rule.interfaces_i.severity = error

dotnet_naming_rule.async_methods_suffix.symbols = async_methods
dotnet_naming_rule.async_methods_suffix.style = async_suffix
dotnet_naming_rule.async_methods_suffix.severity = warning

dotnet_diagnostic.IDE1006.severity = warning   # needed for naming rules to show in build
```

> Watch out: the `Async` suffix rule flags every `async` method without the suffix — including controller actions (`public async Task<IActionResult> Get(...)`) and Blazor event handlers. Consider `suggestion` severity or excluding `BeHealthy.API/Controllers/**` and `BeHealthy.Front/**`.

### Code quality / dead code

```ini
dotnet_diagnostic.IDE0044.severity = warning   # make field readonly
dotnet_diagnostic.IDE0051.severity = warning   # unused private member
dotnet_diagnostic.IDE0052.severity = warning   # private member assigned but never read
dotnet_diagnostic.IDE0059.severity = warning   # unnecessary value assignment
dotnet_diagnostic.IDE0060.severity = warning   # unused parameter
dotnet_diagnostic.IDE0035.severity = warning   # unreachable code
```

### Modern C# style

```ini
# Pattern matching
csharp_style_pattern_matching_over_as_with_null_check = true
dotnet_diagnostic.IDE0019.severity = suggestion
csharp_style_prefer_not_pattern = true
dotnet_diagnostic.IDE0083.severity = suggestion   # use 'is not null'

# Null checks
dotnet_style_coalesce_expression = true
dotnet_diagnostic.IDE0029.severity = suggestion   # use ??
dotnet_style_null_propagation = true
dotnet_diagnostic.IDE0031.severity = suggestion   # use ?.
csharp_style_throw_expression = true
dotnet_diagnostic.IDE0016.severity = suggestion

# Collections / objects
dotnet_style_object_initializer = true
dotnet_diagnostic.IDE0017.severity = suggestion
dotnet_style_collection_initializer = true
dotnet_diagnostic.IDE0028.severity = suggestion
dotnet_diagnostic.IDE0300.severity = suggestion   # collection expressions [..]
dotnet_diagnostic.IDE0090.severity = suggestion   # target-typed new()

# Primary constructors (C# 12)
csharp_style_prefer_primary_constructors = true
dotnet_diagnostic.IDE0290.severity = suggestion

# Expression-bodied members
csharp_style_expression_bodied_methods = when_on_single_line
csharp_style_expression_bodied_properties = true
csharp_style_expression_bodied_constructors = false
```

### `this.` qualification and predefined types

```ini
dotnet_style_qualification_for_field = false
dotnet_style_qualification_for_property = false
dotnet_style_qualification_for_method = false
dotnet_diagnostic.IDE0003.severity = warning   # remove 'this.'

dotnet_style_predefined_type_for_locals_parameters_members = true
dotnet_diagnostic.IDE0049.severity = warning   # 'string' not 'String'
```

### Accessibility modifiers

```ini
dotnet_style_require_accessibility_modifiers = for_non_interface_members
dotnet_diagnostic.IDE0040.severity = warning
```

### Async / reliability analyzers (CA rules)

```ini
dotnet_diagnostic.CA2016.severity = error      # forward CancellationToken (currently warning)
dotnet_diagnostic.CA1849.severity = warning    # call async method in async context
dotnet_diagnostic.CA2012.severity = error      # ValueTask used incorrectly
dotnet_diagnostic.CA1822.severity = suggestion # member can be static
dotnet_diagnostic.CA1848.severity = suggestion # use LoggerMessage delegates
dotnet_diagnostic.CA2254.severity = warning    # logging template should be constant
# CA2007 (ConfigureAwait) — leave off in ASP.NET Core / Blazor apps
```

### Nullable reference types

`<Nullable>enable</Nullable>` is already on in every project. To turn null warnings into build errors:

```xml
<!-- Directory.Build.props -->
<WarningsAsErrors>$(WarningsAsErrors);nullable</WarningsAsErrors>
```

---

## Project-wide build settings (Directory.Build.props)

Options that can go in the root `Directory.Build.props` `<PropertyGroup>`:

```xml
<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>     <!-- already set -->

<!-- Turn on more analyzers. Values: None, Default, Minimum, Recommended, All -->
<AnalysisMode>Recommended</AnalysisMode>
<AnalysisLevel>latest</AnalysisLevel>

<!-- Every warning fails the build (strict; fix existing warnings first) -->
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
<!-- Or only in CI: -->
<TreatWarningsAsErrors Condition="'$(CI)' == 'true'">true</TreatWarningsAsErrors>

<!-- Centralise shared settings -->
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
<LangVersion>latest</LangVersion>
```

To exempt one project (e.g. tests), add to that `.csproj`:

```xml
<PropertyGroup>
  <EnforceCodeStyleInBuild>false</EnforceCodeStyleInBuild>
</PropertyGroup>
```

Or relax rules per folder in `.editorconfig`:

```ini
[BeHealthy.Tests/**.cs]
dotnet_diagnostic.CA1822.severity = none
dotnet_diagnostic.IDE0060.severity = none
```

---

## Suppressing a rule in code

Use sparingly and with a reason.

```csharp
#pragma warning disable IDE0060 // Parameter required by interface contract
public void Handle(int unused) { }
#pragma warning restore IDE0060

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060", Justification = "Interface contract")]
public void Handle(int unused) { }
```

---

## Adopting a new rule — checklist

1. Add the option and `dotnet_diagnostic.<ID>.severity` to `.editorconfig`.
2. Run `dotnet format BeHealthy.sln --diagnostics <ID> --severity info` to fix existing code.
3. Review the diff (`git diff --stat`), revert anything that looks wrong.
4. `dotnet build BeHealthy.sln` — 0 errors.
5. `dotnet test BeHealthy.Tests` — all green.
6. Commit the `.editorconfig` change together with the reformatted code.

## References

- Code-style rules (IDExxxx): https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules/
- Formatting options: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules/csharp-formatting-options
- Naming rules: https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules/naming-rules
- Quality rules (CAxxxx): https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/
