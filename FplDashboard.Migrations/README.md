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

4. **Best Practices**
   - Do not add migrations to the `FplDashboard.DataModel` project.
   - Always use this migrations project for schema changes.
   - Keep the migrations project up to date with model changes in `FplDashboard.DataModel`.

### Troubleshooting
- If you encounter errors about missing context or configuration, ensure the `FplDashboard.Migrations` project references `FplDashboard.DataModel` and has a valid `appsettings.json`.
- If you change the model structure, always add a new migration and review the generated code for correctness.

---

## Removing Old Migrations

If you previously created migrations in `FplDashboard.DataModel`, move them to this project and delete them from the old location to avoid confusion.

---

## Useful Commands

- List migrations:
  ```sh
  dotnet ef migrations list --project FplDashboard.Migrations --startup-project FplDashboard.Migrations
  ```
- Remove last migration:
  ```sh
  dotnet ef migrations remove --project FplDashboard.Migrations --startup-project FplDashboard.Migrations
  ```

---

For more details, see the [EF Core documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli).
