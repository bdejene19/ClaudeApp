# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

This repo contains a single app, `claude-app/`, a Next.js project. All commands below are run from that directory.

## Commands

```bash
cd claude-app
npm run dev     # start dev server (Turbopack) at http://localhost:3000
npm run build   # production build
npm run start   # serve the production build
npm run lint    # eslint
```

There is no test suite configured yet (no test script/framework in `package.json`).

## Important: pre-release Next.js version

`claude-app` pins `next@16.3.2`, a version ahead of what most training data covers — APIs, conventions, and file structure may differ from what you expect. Before writing Next.js code here, check the relevant guide under `claude-app/node_modules/next/dist/docs/` and heed any deprecation notices. This guidance lives in `claude-app/AGENTS.md`, which `next dev` regenerates automatically — if it reappears in a diff after being removed, that's expected; commit it along with your other changes rather than stripping it again.

## Architecture

- Next.js App Router, TypeScript (strict), Tailwind CSS v4 (via `@tailwindcss/postcss`), ESLint flat config (`eslint-config-next`).
- `src/app/` holds the App Router tree — currently just the default scaffold (`layout.tsx`, `page.tsx`, `globals.css`) from `create-next-app`, unmodified.
- Path alias `@/*` maps to `claude-app/src/*` (see `tsconfig.json`).
