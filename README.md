# MVCSinglePage

A sample ASP.NET Core MVC application for managing people and products. It combines Razor views with JavaScript-driven requests and uses Entity Framework Core with SQL Server for data access.

## Features

- Browse and manage people and products
- Create, edit, view details, and delete records through controller endpoints
- Separate controllers, application services, DTOs, repositories, and domain models
- Asynchronous database operations with Entity Framework Core
- Razor views with Bootstrap and client-side JavaScript

## Technology Stack

- .NET 10 / ASP.NET Core MVC
- Entity Framework Core 10
- Microsoft SQL Server
- Razor, JavaScript, and Bootstrap

## Project Structure

```text
MVCSinglePage/
├── ApplicationServices/
│   ├── Dtos/                 # Request and response DTOs
│   └── Services/             # Application services and contracts
├── Controllers/              # MVC pages and JSON endpoints
├── Migrations/               # EF Core database migrations
├── Models/
│   ├── DomainModels/         # Person and Product entities
│   ├── Services/             # Repository contracts and implementations
│   └── ProjectDbContext.cs   # EF Core database context
├── Views/                    # Razor pages
└── wwwroot/                  # CSS, JavaScript, and static libraries
```

## Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server LocalDB
- (Optional) EF Core CLI tools for applying migrations

## Getting Started

1. Clone the repository and open a terminal in its root directory.
2. Configure the `DefaultConnection` connection string in `MVCSinglePage/appsettings.json`, or provide it through .NET user secrets or an environment-specific configuration source. The checked-in value uses a local SQL Server instance and Windows authentication; adjust it for your environment.
3. Restore dependencies and apply the database migrations:

   ```powershell
   dotnet restore MVCSinglePage/MVCSinglePage.csproj
   dotnet tool install --global dotnet-ef
   dotnet ef database update --project MVCSinglePage/MVCSinglePage.csproj
   ```

   If `dotnet-ef` is already installed, skip the tool installation command.
4. Run the application:

   ```powershell
   dotnet run --project MVCSinglePage/MVCSinglePage.csproj
   ```

5. Open the local URL printed in the terminal. The default route opens the home page. The people and products pages are available at `/Person` and `/Product`.

## Main Routes

| Route | Purpose |
| --- | --- |
| `/Person` | People management page |
| `/Person/GetAll` | Returns the people list as JSON |
| `/Person/GetPersonDetails?id=...` | Returns a person's details |
| `/Person/create` | Creates a person (JSON `POST`) |
| `/Person/Edit` | Updates a person (JSON `POST`) |
| `/Person/Delete` | Deletes a person (JSON `POST`) |
| `/Product` | Products page |
| `/Product/GetAll` | Returns the products list as JSON |
| `/Product/GetProductDetails?id=...` | Returns a product's details |
| `/Product/Create` | Creates a product (JSON `POST`) |
| `/Product/Edit` | Updates a product (JSON `POST`) |
| `/Product/Delete` | Deletes a product (JSON `POST`) |

The create, edit, and delete endpoints accept JSON request bodies. See the DTOs under `ApplicationServices/Dtos` for the expected fields.

## Database Migrations

After changing the data model, create a migration and apply it with:

```powershell
dotnet ef migrations add <MigrationName> --project MVCSinglePage/MVCSinglePage.csproj
dotnet ef database update --project MVCSinglePage/MVCSinglePage.csproj
```

## License

No license is currently specified. Add a `LICENSE` file if you intend to grant reuse rights.
