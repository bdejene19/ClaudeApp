---
name: git-commit
description: Commit the current changes to an appropriate branch and push them — creating a new branch (asking the user for its name) if currently on main/master/dev/development, and always confirming with the user before pushing. Use when the user asks to commit code, commit these changes, save changes to a branch, or push a commit.
---

Commits the current changes to an appropriate branch and pushes them, following this user's git workflow conventions.

## Steps

1. **Check status and branch.** Run `git status`, `git branch --show-current`, `git diff`, and `git diff --staged` to see the current branch and what has changed (staged, unstaged, and untracked).

2. **Handle protected branches.** If the current branch is `main`, `master`, `dev`, or `development`, ask the user for a new branch name (do not invent one yourself), then create and switch to it with `git checkout -b <name>`. Otherwise, stay on the current branch and commit there directly.

3. **Stage changes.** Add the specific files that are part of this change by name (never `git add -A` or `git add .`). If anything looks like it might contain secrets (`.env`, credentials, keys), stop and warn the user instead of staging it.

4. **Draft and create the commit.** Write a concise, "why"-focused commit message (1-2 sentences) that explains the motivation for the change, not a list of what changed. Create the commit via a heredoc, ending with:
   ```
   Co-Authored-By: Claude Sonnet 5 <noreply@anthropic.com>
   ```

5. **Confirm before pushing.** Show the user the branch name, the remote it will push to, and the commit(s) about to be pushed. Explicitly ask them to confirm before running `git push` — do not push without this confirmation, even though invoking this skill implies intent to commit.

6. **Push.** Once confirmed, run `git push -u origin <branch>` if the branch was newly created in step 2, otherwise plain `git push`.

7. **Report the result.** Tell the user the branch name, the commit hash, and whether the push succeeded.

## What this skill deliberately does NOT do

- Does not open a pull request — stops after pushing.
- Does not force-push, amend existing commits, or skip hooks (`--no-verify`).
- Does not auto-generate branch names — always asks the user when a new branch is needed.
- Does not `git add -A`/`git add .` — stages specific files only.

Don't add any of the above unless the user explicitly requests it for this run.
