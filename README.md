# FplDashboard

FplDashboard is a multi-project solution for managing, analyzing, and displaying Fantasy Premier League (FPL) data. The solution is organized into several key projects:

- **FplDashboard.API**  
  ASP.NET Core Web API that exposes endpoints for dashboard data, teams, players, and related features.

- **FplDashboard.DataModel**  
  Contains Entity Framework Core data models and the database context for FPL data storage.

- **FplDashboard.ETL**  
  Handles data extraction, transformation, and loading (ETL) from external FPL sources into the database.

- **FplDashboard.UI**  
  Frontend user interface (UI) for interacting with the dashboard (located in the `fpl-dashboard-ui` subfolder).

- **FplDashboard.Migrations**  
  Manages database migrations and schema updates.
