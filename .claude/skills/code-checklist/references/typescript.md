# TypeScript / JavaScript golden standard

**Cited standards**: [Airbnb JavaScript Style Guide](https://github.com/airbnb/javascript) as the general baseline, deferring to the project's own `eslint-config-next` (and any local `.eslintrc`/`eslint.config.*`) wherever it disagrees with Airbnb — the repo's own lint config always wins. TypeScript-specific rules follow the Airbnb TypeScript guide plus `strict` mode conventions.

## Style & formatting

- `const`/`let` only, never `var`.
- Prefer arrow functions for callbacks and anonymous functions; use `function` declarations for named, hoisted top-level functions.
- Strict equality (`===`/`!==`) always; no `==`/`!=` except `== null` for the null-or-undefined check.
- Template literals over string concatenation.
- Destructure objects/arrays where it improves readability; avoid destructuring that obscures the source shape.
- Explicit return types on exported functions; inference is fine for local/private helpers.
- No `any` — use `unknown` with narrowing, a proper union, or a generic. Flag every `any` as a violation unless it's justified with an inline comment explaining why it's unavoidable.
- Avoid non-null assertions (`!`) except where the surrounding code makes null-safety provably impossible to express otherwise.
- One component/class per file for React components; file name matches the exported symbol (PascalCase for components, camelCase for hooks/utilities).
- Imports ordered: external packages, then absolute (`@/*`) imports, then relative imports, with a blank line between groups.

## Access-modifier conventions (public/private/protected)

TS/JS doesn't have `public`/`private`/`protected` in plain JS, but in TypeScript classes and in module-level `export`, apply:

- **Public (exported symbols / `public` class members)**: require a JSDoc comment on every exported function, class, and public class member describing purpose, params, and return — unless the name and types alone are fully self-explanatory (e.g. a trivial getter). Changes to an exported function's signature are **higher-risk**: flag them explicitly as a potential breaking change for downstream importers and check for corresponding call-site updates.
- **Private (`private` class members, non-exported module symbols, `#field` private class fields)**: no doc-comment requirement. Prefer `#privateField` (true private fields) over the `private` TypeScript keyword for runtime-enforced privacy where the target supports it; otherwise the `private` keyword is acceptable.
- **Protected**: require a short comment explaining the contract subclasses are expected to fulfill, since `protected` members are an extension point.
- **Encapsulation check**: flag any `public`/exported member that is never imported/referenced outside its own file — it should likely be private or unexported. Flag any `protected` member with no subclass in the codebase — likely should be `private`.

## Code scope/layer rules

- **Frontend/UI** (`src/app/**`, `src/components/**`, anything returning JSX): components must be typed with explicit prop interfaces/types (no inline anonymous prop types on exported components); no inline styles unless justified (Tailwind classes preferred, matching this repo's Tailwind v4 setup); side effects in `useEffect` must list complete, correct dependency arrays.
- **Backend/API** (`src/app/api/**`, route handlers, server actions): validate and type all external input (request bodies, query params) before use — flag any handler that reads `request.json()`/params without a type guard or schema check; errors must be caught and turned into proper HTTP responses, not allowed to throw uncaught.
- **Infra/config** (`next.config.*`, `eslint.config.*`, `tsconfig.json`, CI/workflow files): changes here get flagged for extra scrutiny regardless of size — these affect the whole build/deploy pipeline. Confirm any new dependency or compiler-option change is intentional and documented in the diff/PR description.
- **Data** (API clients, data-fetching utilities, any DB/ORM layer added later): flag any data-access function that isn't isolated behind a typed interface, and any raw/unescaped data passed toward rendering or a query without sanitization.
