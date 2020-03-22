# PayX - Payment Gateway API

PayX is a Payment Gateway API designed to easily allow shoppers to process bank payments.

## The project

The solution is a multi-layer API designed in 5 projects.
The main point behind this approach is that we can provide a better separation of concerns and to decouple one project from another.

### PayX.Api

This is our API entry point.

### PayX.Service

This is our business logic layer.

### PayX.Data

This is our persistence layer, where we make connections to any data persistence provider (Databases, JSON Files, ...).

### PayX.Core

This is our application’s foundation, it will hold our contracts (interfaces, …), our models and everything else that is essential for our application to work.

### PayX.Bank

This is our Acquiring Bank implementation (here we are mocking a banking service).

## Dependencies

* (.NET Core SDK 3.1.200)[https://dotnet.microsoft.com/download/dotnet-core/3.1] - C# Backend Framework
* (Entity Framework Core 3.1.2)[https://github.com/dotnet/efcore] - Object-database mapper
* (Serilog 3.2.0)[https://github.com/serilog/serilog] - Logging library
* (Prometheus-net 3.5.0)[https://github.com/prometheus-net/prometheus-net] - Metrics libray
* (Swashbuckle 5.1.0)[https://github.com/domaindrivendev/Swashbuckle.AspNetCore] - ASPNET.Core API documentation