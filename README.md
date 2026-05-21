# 📌 BlogSystem API

## Overview

**BlogSystem** is a modern ASP.NET Core web application that combines both **MVC** and **REST API** functionality.  
It allows users to **register, login, and create blog posts** while demonstrating real-world backend architecture practices.

The project is built with a focus on **clean structure, scalability, and maintainability**.

## Architecture

The project follows a **Layered Architecture (Clean Architecture style)**:

- **BlogSystem.Web** → MVC Controllers, Views, API endpoints, Swagger UI  
- **BlogSystem.Application** → Business logic, DTOs, services, validators  
- **BlogSystem.Domain** → Core entities and domain models  
- **BlogSystem.Infrastructure** → Database access, EF Core, repositories
- BlogSystem.Tests → Unit tests

### Key Principles:
- Separation of concerns  
- Dependency Injection  
- Scalable design  
- Maintainable structure  

## Technologies Used

- ASP.NET Core MVC / Web API  
- Entity Framework Core (Code First)  
- SQL Server  
- AutoMapper  
- FluentValidation  
- JWT Authentication  
- Cookie Authentication (MVC)  
- Swagger / OpenAPI  
- Dependency Injection
- Unit Tests (xUnit, Moq)
- C# (.NET 8)

## Features

- User Registration & Login  
- Role-based authorization (Admin / User)  
- Cookie-based authentication (MVC)  
- JWT authentication (API support)  
- Create / Read blog posts  
- Input validation with FluentValidation  
- Global exception handling  
- Swagger API documentation

## Getting Started

### Requirements
- .NET 8 SDK  
- SQL Server  

### Run the project

```bash
dotnet restore
dotnet ef database update
dotnet run
```

👤 Author<br>
Nino Kinkriashvili<br>
.NET | Backend Developer (Learning Project) 🚀
