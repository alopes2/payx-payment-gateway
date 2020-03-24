# PayX - Payment Gateway API [![Build Status](https://travis-ci.com/alopes2/payx-payment-gateway.svg?branch=master)](https://travis-ci.com/alopes2/payx-payment-gateway) [![codecov](https://codecov.io/gh/alopes2/payx-payment-gateway/branch/master/graph/badge.svg)](https://codecov.io/gh/alopes2/payx-payment-gateway)

PayX is a Payment Gateway API designed to easily allow shoppers to process bank payments.

## Requirements

* .NET Core SDK 3.1.200
* Microsoft SQL Server 2019 - Developer Edition
* Docker (For container image management)

## Setup

### Work directory

Navigate to the `src` folder and use it as the work directory in your command line.


### Environment

Set your .NET Core environment to Development by setting the Enviroement Variable `ASPNETCORE_ENVIRONMENT` to `Development`.

### Database configuration

Check your `server` database in the `Default` property of the `ConnectionStrings` section in `appsetting.Development.json`, mine is `localhost` because of SQL Server 2019.
If you are using SQL Express it should be `.\SQLExpress`.

Change the `user id` and `password` for a user with admin access in your SQL Server database.
You can also user `sa` user and change its password to the one I'm using here `MyComplexPassword!234`.

In my file I have this:

```
server=localhost; database=PayX; user id=sa; password=MyComplexPassword!234
```

Where `server` is my database server, `database` is the PayX database, `user id` is a user with admin access to your database server and `password` is this users password.

### Dependencies restore

Restore all packages with:

```
dotnet restore
```

### Create database

Now let's create our database structure with:

```
dotnet ef --startup-project ./PayX.Api/PayX.Api.csproj database update
```

### Application run

To run the application just run the following command:

```
dotnet run -p ./PayX.Api/PayX.Api.csproj
```

You can access the application on the follwing URL:

```
http://localhost:5000
```

### Authenticate

A default **Admin** user is seeded to the database. You can authenticated by entering the following credentials in the `/signin` endpoint through swagger UI:

```
email: admin@payx.io
password: 1234
```

The endpoint will return a valid JWT Token. You can copy it then click on the `Authorize` green button
and then enter the words `Bearer` following by the JWT Token, like the example below:

```
Bearer <JWT_TOKEN>
```

Then click on the small `Authorize` green button and then on `Close`.
Now you should be authenticated to use the API.

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

### PayX.UnitTests

Here are the **Unit Tests** for our projects. To run them just go to the `src` folder and run:

`dotnet test`

It is also set to generate a code coverage file with `coverlet` . To generate a coverage file just run:

`dotnet test ./src/PayX.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover`

This will generate a `coverage.opencover.xml` file in the `PayX.UnitTests` project folder.

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

### Docker

A Dockerfile was written to allow the build of docker production image.

### Continuous Integration Pipeline

A continuous integration pipeline was set using [Travis CI](https://travis-ci.com/) .
It is set to build and tests the application.
It generates and pushes a docker production image to docker hub.
It also generates and push our code coverage to [Codecov](https://codecov.io/) .

You can check pipeline and code coverage status through the **badges** in this README file.

## Dependencies

* [.NET Core SDK 3.1.200](https://dotnet.microsoft.com/download/dotnet-core/3.1) - C# Backend Framework
* [Entity Framework Core 3.1.2](https://github.com/dotnet/efcore) - Object-database mapper
* [Serilog 3.2.0](https://github.com/serilog/serilog) - Logging package
* [Prometheus-net 3.5.0](https://github.com/prometheus-net/prometheus-net) - Metrics package
* [Swashbuckle 5.1.0](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - ASPNET.Core API documentation
* [AutoMapper 9.0.0](https://github.com/AutoMapper/AutoMapper) - Mapping package
* [Coverlet](https://github.com/tonerdo/coverlet) - Code coverage generation package - **coverlet.msbuild** and **coverlet.collector** are the depencies packages needed
* [BCrypt 3.3.3](https://github.com/BcryptNet/bcrypt.net) - Encryption package

## Improvement Points

### Code Coverage - Tests

More tests can be written to increase code coverage and confiability.

### Authorization

Authorization is done in a basic **login-password** approach, it could be improved in a implementation where the Merchant wouldn't need to login to get a new JWT token.

### Encryption

Encryption could be added for saving critical payment information and/or Acquiring Bank connections.

### Throttling

Throttling could be applied for limitting number of requests per second to protect against attacks.