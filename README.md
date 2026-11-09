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
