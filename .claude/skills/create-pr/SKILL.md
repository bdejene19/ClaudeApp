---
name: create-pr
description: Push the current branch and open a GitHub pull request, auto-targeting "main" or "dev" (preferring main if both exist), or an explicit base branch passed as an argument. Use when the user asks to create, open, or make a pull request / PR for the current branch's changes.
---

Push whatever's committed on the current branch and open a GitHub pull request for it with `gh`.

## Steps

1. **Determine the current branch** via `git branch --show-current`.

2. **Guardrail check.** If the current branch is `main`, `master`, `dev`, or `development`, stop immediately and tell the user this skill can't open a PR from a protected/mainline branch — ask them to switch to (or create) a feature branch first. Do not proceed past this step on a protected branch.

3. **Resolve the base branch:**
   - If an argument was passed to this skill, use it directly as the base branch and skip the auto-detect below.
   - Otherwise, check `origin` for `main` and `dev`:
     ```
     git ls-remote --heads origin main dev
     ```
     - If both exist, target `main`.
     - If only one exists, target that one.
     - If neither exists, abort with a clear error — do not create or push a new base branch.

4. **Check for an existing open PR** for this head branch to avoid creating a duplicate:
   ```
   gh pr list --head <branch> --state open
   ```
   If one already exists, report its URL and stop — don't create a second PR.

5. **Push the current branch** if it has commits not yet on `origin`:
   ```
   git push -u origin <branch>
   ```

6. **Create the PR**, letting `gh` auto-generate the title/body from the branch's commits rather than prompting the user:
   ```
   gh pr create --base <base> --head <branch> --fill
   ```

7. **Report back** the PR URL exactly as returned by `gh pr create`.

## What this skill deliberately does NOT do

- Does not commit or stage any changes — that's a separate concern (e.g. the `git-commit` skill), not this one's job.
- Does not create or push a missing base branch.
- Does not run when the current branch is `main`, `master`, `dev`, or `development`.
- Does not open a second PR when one is already open for the branch.

Don't deviate from the above unless the user explicitly asks for different behavior for this run.
