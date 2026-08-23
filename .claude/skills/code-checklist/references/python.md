# Python golden standard

**Cited standards**: [PEP 8](https://peps.python.org/pep-0008/) for style, [PEP 257](https://peps.python.org/pep-0257/) for docstring conventions, and the [Google Python Style Guide](https://google.github.io/styleguide/pyguide.html) as the gap-filler for anything PEP 8/257 doesn't cover (imports, comprehensions, exceptions, type annotations).

## Style & formatting

- 4-space indentation, no tabs. Lines ≤ 79 characters (PEP 8) — flag lines that exceed this unless the project has an explicit configured line-length override (check for a `pyproject.toml`/`setup.cfg`/`.flake8` line-length setting before flagging).
- `snake_case` for functions, methods, variables, and modules; `PascalCase` for classes; `UPPER_SNAKE_CASE` for constants.
- Two blank lines between top-level definitions, one blank line between methods inside a class (PEP 8).
- Imports: standard library, then third-party, then local — each group alphabetized, separated by a blank line (Google style); no wildcard imports (`from x import *`).
- Prefer f-strings over `%`-formatting or `.format()`.
- List/dict/set comprehensions preferred over `map`/`filter` with `lambda`, but flag comprehensions that nest more than 2 levels or exceed ~1 line of readability — prefer an explicit loop instead (Google style guidance).
- Explicit exception types in `except` clauses; never a bare `except:`.
- Type hints on all function signatures (params and return) for anything beyond trivial internal scripts — flag public/exported functions missing them.

## Access-modifier conventions (public/private/protected)

Python has no enforced access modifiers — apply the naming-convention equivalent:

- **Public** (no leading underscore): this is the module/class's public API. Requires a PEP 257–compliant docstring (one-line summary, blank line, then details/Args/Returns/Raises as needed) on every public module, class, and function — flag any public function/class without one. Changes to a public function's signature are **higher-risk**: flag as a potential breaking change for importers.
- **Private (`_leading_underscore`)**: convention-only "internal use" marker — no docstring required, but flag any external module importing a `_private` name, since that violates the convention's contract.
- **Protected-equivalent / subclass API (`_single_underscore` on methods intended for subclass override)**: require a short comment or docstring noting it's an extension point for subclasses, similar to `protected` in other languages.
- **Name-mangled "strongly private" (`__dunder` non-magic names)**: flag use of this pattern for anything other than genuinely avoiding subclass name collisions — it's rarely needed and often overused.
- **Encapsulation check**: flag any public (no-underscore) function/attribute that's never referenced outside its defining module — likely should be prefixed `_private`.

## Code scope/layer rules

- **Frontend/UI** (if the Python code renders UI, e.g. a Django/Flask template layer or a CLI's presentation layer): keep formatting/presentation logic separate from business logic; flag view functions that also perform data validation or persistence inline.
- **Backend/API** (route handlers, service/controller layers, FastAPI/Flask/Django views): validate and type all external input before use — flag any handler reading request data without a schema/validation step (e.g. missing Pydantic model, missing `request.get_json()` type check); ensure exceptions are caught and mapped to proper HTTP error responses, not left to propagate as raw 500s.
- **Infra/config** (`pyproject.toml`, `setup.cfg`, CI workflow files, Dockerfiles): flag any change here for extra scrutiny — dependency version bumps, build config changes affect the whole pipeline and should be called out explicitly in the report.
- **Data** (DB models, ORM layers, migrations, data-access functions): flag any raw SQL string built via f-string/`%`/concatenation with untrusted input (SQL-injection risk — use parameterized queries/ORM); flag data-access functions not isolated behind a clear function/class boundary.
