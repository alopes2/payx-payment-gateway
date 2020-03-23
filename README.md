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

## Features

### Repository and Unit of Work pattern

For data management it is used the Repository and Unit Of Work patterns to decouple and absctract our business logic from our data layer.

### Authorization

Authorization was implemented in a simplistic way to link Payments to Users. Making it impossible for other users see payments that are not theirs.

It is possible for a user to sign up and sign in through the Auth endpoints.

Payments endpoints were protected to be only accessible by Merchants or Admins. This was implemented with a policy "CanManagePayments".

Only Admin users can create currencies.

### Logging

Logging was implemented using Serilog package.

It was configured to log to the console and to log to a file.
File location can be configured by changing the *FilePath* property in the *Serilog* section of the appsettings.json.

Default is set to */var/PayX/Logs/log.log*, which is translated to *C:\var\PayX\Logs\log.log* on Windows OS.

### Global Exception Handling

A middleware was set to globally handle any exception that is thrown inside the application.
It's purpose is to log the exception using our logging implementation and return a formatted response to the user.

### Metrics

Metrics were implemented using *Prometheus-net* package.

The default metrics and Http metrics were set in `Startup.cs` .
Also a custom metrics was set for counting the number of Exceptions thrown in the application. It was set through the `MetricsService.cs` for setting a Prometheus counter and applied in a Metrics Middleware using `MetricsMiddleware.cs`.

Note that if more metrics are required, you just need to add a property to `MetricsService.cs` and initialize it in there with Prometheus-net.

### Object Mapping

Mappings between Domain and Resource models is done using AutoMapper package.

Mapping profile is defined in the `Mappings` folder in `MappingProfile.cs`.

## Dependencies

* [.NET Core SDK 3.1.200](https://dotnet.microsoft.com/download/dotnet-core/3.1) - C# Backend Framework
* [Entity Framework Core 3.1.2](https://github.com/dotnet/efcore) - Object-database mapper
* [Serilog 3.2.0](https://github.com/serilog/serilog) - Logging package
* [Prometheus-net 3.5.0](https://github.com/prometheus-net/prometheus-net) - Metrics package
* [Swashbuckle 5.1.0](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - ASPNET.Core API documentation
* [AutoMapper 9.0.0](https://github.com/AutoMapper/AutoMapper) - Mapping package