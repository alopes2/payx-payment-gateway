FROM mcr.microsoft.com/dotnet/core/sdk:3.1.200-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/PayX.Api/*.csproj ./PayX.Api/
COPY src/PayX.Core/*.csproj ./PayX.Core/
COPY src/PayX.Service/*.csproj ./PayX.Service/
COPY src/PayX.Bank/*.csproj ./PayX.Bank/
COPY src/PayX.Data/*.csproj ./PayX.Data/
COPY src/PayX.UnitTests/*.csproj ./PayX.UnitTests/
COPY src/*.sln ./
RUN dotnet restore

# Copy everything else and build
COPY src ./
RUN ls
RUN dotnet build -c Release -o build
RUN dotnet publish ./PayX.Api/PayX.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "PayX.Api.dll"]