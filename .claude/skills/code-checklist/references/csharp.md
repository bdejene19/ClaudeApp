# C# / .NET golden standard

**Cited standards**: [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) and the [.NET framework design guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/) for naming and API design.

## Style & formatting

- 4-space indentation, no tabs. Allman brace style (opening brace on its own line) per Microsoft convention.
- `PascalCase` for types, namespaces, methods, properties, events, and public fields/constants; `camelCase` for local variables, parameters, and private fields (with a `_camelCase` leading underscore common and acceptable for private instance fields).
- `var` only when the type is obvious from the right-hand side (e.g. `var list = new List<string>();`); use an explicit type when it aids readability (e.g. numeric literals, method return types that aren't self-evident).
- `is`/`as` pattern-matching preferred over explicit casts where applicable; use expression-bodied members for simple one-line properties/methods.
- `async`/`await` all the way down — flag any `async void` outside event handlers, and flag `.Result`/`.Wait()` blocking calls on a `Task` (deadlock risk), preferring `await`.
- Use `string.IsNullOrEmpty`/`IsNullOrWhiteSpace` rather than manual null/length checks.
- Nullable reference types (`?`) should be enabled and respected — flag code that dereferences a nullable without a null check when nullable annotations are on.

## Access-modifier conventions (public/private/protected)

C# has real, enforced access modifiers — apply the language's own documentation and design-guideline conventions directly:

- **`public`**: requires an XML doc comment (`/// <summary>`, `<param>`, `<returns>`) on every public type and member — flag any public API surface missing one. Follow .NET design guidelines: public members should not expose mutable collections directly (return `IReadOnlyList<T>`/`IEnumerable<T>` instead of `List<T>`), and public method signature changes are **higher-risk** — flag as a potential breaking change for consumers, especially in a published/versioned library.
- **`private`**: no doc-comment requirement; `_camelCase` naming for private fields is the expected convention — flag private fields that don't follow it.
- **`protected`** / **`protected internal`**: require a short XML doc or comment describing the contract for derived classes, since these are the class's extension points. Flag `protected` members with no subclass anywhere in the codebase — likely should be `private`.
- **`internal`**: treat like a scoped-public — assembly-internal consumers still benefit from a doc comment if the member is non-trivial, though it's not mandatory like `public`.
- **Encapsulation check**: flag any `public` member never referenced outside its declaring class/assembly (candidate for `private`/`internal`), and flag mutable `public` fields that should be properties with controlled setters.

## Code scope/layer rules

- **Frontend/UI** (Blazor components, WPF/MAUI XAML code-behind, Razor views): keep UI event handlers thin — flag business logic embedded directly in a code-behind/component event handler instead of delegated to a service.
- **Backend/API** (ASP.NET Core controllers/minimal API endpoints, services): validate and model-bind all external input (flag any endpoint reading raw request data without model binding/validation attributes or FluentValidation); ensure exceptions are handled via middleware/filters rather than leaking raw stack traces in responses; controllers should delegate to a service/business layer rather than embedding logic directly.
- **Infra/config** (`.csproj`, `appsettings*.json`, Dockerfiles, CI YAML): flag any change here for extra scrutiny — package reference/version changes, connection strings, and pipeline config affect the whole build/deploy and should be called out explicitly in the report. Never flag missing docs on config files, but do flag secrets/connection strings committed in plaintext in `appsettings.json` (should be in `appsettings.Development.json`, user secrets, or an environment/vault-backed config).
- **Data** (EF Core `DbContext`, entities, migrations, repositories): flag any raw SQL built via string interpolation/concatenation with unparameterized external input (SQL-injection risk — use `FromSqlInterpolated`/parameterized queries); flag entity classes with public settable navigation properties that bypass intended aggregate boundaries; migrations should be reviewed for irreversible operations (column drops, type narrowing) called out explicitly.
