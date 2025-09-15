# FplDashboard Migrations

## Managing Entity Framework Core Migrations

This project is dedicated to managing database schema migrations for the FplDashboard solution. It references the `FplDashboard.DataModel` project, which contains the `FplDashboardDbContext` and all entity models.

### Workflow

1. **Add a Migration**

   Run the following command from the solution root:

   ```sh
   dotnet ef migrations add <MigrationName> --project FplDashboard.Migrations --startup-project FplDashboard.Migrations
   ```
   - This will create a new migration in `FplDashboard.Migrations/Migrations`.

2. **Apply Migrations (Update the Database)**

   You can apply all pending migrations to your database by running the migrations console app:

   ```sh
   dotnet run --project FplDashboard.Migrations
   ```
   This will use the connection string in `FplDashboard.Migrations/appsettings.json`.

   Alternatively, you can use the EF CLI:
   ```sh
   dotnet ef database update --project FplDashboard.Migrations --startup-project FplDashboard.Migrations
   ```

3. **Configuration**

   - The connection string is set in `FplDashboard.Migrations/appsettings.json` under the `ConnectionStrings` section.
   - Ensure this file is present and correctly configured before running migration commands.