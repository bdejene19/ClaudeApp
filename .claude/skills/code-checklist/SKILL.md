---
name: code-checklist
description: Review code against golden coding-standard checklists for TypeScript/JavaScript, Python, and C#/.NET, scoped by code layer (frontend/backend/infra/data) and by public/private/protected access-modifier conventions. Use when the user asks to check coding standards, run a style/standards checklist, or verify a diff conforms to golden standards.
---

Walk a diff or a set of files against language-specific "golden standard" coding conventions and report where they fall short — a standards-conformance checklist, not a bug hunt.

## Steps

1. **Determine the target.** If the user's invocation included a path, PR, or other target, use it. If invoked bare, ask the user whether to check the current git diff (`git diff`, or `git diff main...HEAD`/`git diff <default-branch>...HEAD` if on a feature branch) or a specific folder/file path — don't default silently.

2. **Detect the language(s) in scope** from file extensions in the target:
   - `.ts`, `.tsx`, `.js`, `.jsx` → TypeScript/JavaScript
   - `.py` → Python
   - `.cs` → C#/.NET
   For any file whose language has no reference file below, list it as skipped ("no golden standard defined for this file type") rather than guessing at standards for it.

3. **Load only the matching reference file(s)** for the languages actually detected — `references/typescript.md`, `references/python.md`, `references/csharp.md` — to keep the standards in scope small and relevant.

4. **Walk each file against three rule groups** from the loaded reference:
   - **Style-guide conformance** — the language's cited golden standard.
   - **Code scope/layer rules** — apply the reference's frontend/UI vs. backend/API vs. infra/config vs. data/db guidance based on where the file sits (e.g. `src/app/**` or components = frontend, API routes/controllers = backend, `*.config.*`/`appsettings*`/CI files = infra, migrations/models/repositories = data).
   - **Access-modifier rules** — for each `public`/`private`/`protected` (or language-equivalent) member touched, apply that language's own convention for documentation, naming, API-stability risk, and encapsulation correctness.

5. **Produce a structured checklist report**, grouped by category (Style, Layer, Access-modifier), each item marked pass or flagged. Flagged items must cite `file:line` and name the specific standard violated (e.g. "PEP 8: line too long (92 > 79 chars)"). Print this directly as markdown — do not use the `ReportFindings` tool, and do not apply fixes automatically.

## What this skill deliberately does NOT do

- Does not hunt for logic/correctness bugs, security issues, or simplification opportunities — that's `/code-review`'s job, not this skill's.
- Does not apply fixes automatically, even for trivial violations.
- Does not invent or guess standards for languages outside TypeScript/JavaScript, Python, and C#/.NET.
- Does not assume a default target (diff vs. path) when invoked with no argument — always asks.

Don't add any of the above unless the user explicitly requests it for this run.
