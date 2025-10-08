# Copilot Instructions for FplDashboard.UI

## Project Overview
- This is an Angular 20 application for Fantasy Premier League dashboards, generated with Angular CLI.
- Main features include player stats, team fixtures, and news, with a focus on paged data grids and modern UI.
- The codebase is organized by feature modules (e.g., `players`, `teams`, `dashboard`).

## Key Architectural Patterns
- **API Communication:**
  - All backend API calls are centralized in `src/app/api-data.service.ts`.
  - Models for API responses are in feature folders (e.g., `players/player-paged.model.ts`).
- **Component Structure:**
  - Use standalone Angular components (no NgModules).
  - Feature folders contain related components, models, and styles. Given we are using vertical architecture, avoid shared components unless absolutely necessary or it makes sense to. If you do create a shared component, put it in the `shared` folder and raise it clearly in the chat.
- **UI Patterns:**
  - Data tables use sticky columns for key fields (see `players.component.html`/`.scss`).
  - Loading states use a shared `LoadingSpinnerComponent` (see `shared/loading-spinner.component.ts`).
  - Pipes (e.g., `cost.pipe.ts`) are used for data formatting in tables, always introduce a pipe where it makes sense to.

## Project-Specific Conventions
- **Styling:**
  - Use SCSS, with feature-specific styles in each component folder.
  - Page layouts use consistent padding and header styles (see `teams.component.scss`, `players.component.scss`).
- **Loading:**
  - All main pages use a centered spinner for loading states, with `@if` blocks (Angular v17+ syntax).
- **Error Handling:**
  - Error states are shown inline with icons and messages (see `dashboard.component.html`).

## Integration Points
- **Backend:**
  - API endpoints are assumed to be RESTful and paged; see `api-data.service.ts` for usage.
- **Pipes:**
  - Custom pipes (e.g., `cost.pipe.ts`) are used for domain-specific formatting.

## Examples
- To add a new paged API endpoint:
  1. Add the model to the relevant feature folder.
  2. Add a method to `api-data.service.ts`.
  3. Update the relevant component to fetch and display the data.
- To update table UI conventions, see `players.component.html` and `players.component.scss` for sticky column and header patterns.

---

For questions about conventions or unclear patterns, review the feature folders and shared components for examples. If a pattern is not documented here, prefer the approach used in the most recent feature (e.g., `players`).

---

When generating code, ensure it adheres to the above conventions and patterns. Always prioritize consistency with existing code over introducing new patterns unless explicitly requested.

Code should be concise, maintainable, and follow Angular best practices. It should always build and run without errors or warnings. Try to avoid unnecessary complexity and minimize the number of comments unless they add value.

If anything changes in the project structure or conventions, please update this document accordingly.

When making suggestions ensure you have verified the suggestion exists and follows best practices, this should be done by checking online. 

When installing packages, ensure you are using the latest stable versions that is compatibale with the other packages in the project.