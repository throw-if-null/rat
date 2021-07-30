# [Rat Api](https://github.com/throw-if-null/rat/blob/branch-0/Rat/documentation/api/rat-api.md)
[![CI](https://github.com/throw-if-null/rat.api/actions/workflows/ci.yml/badge.svg)](https://github.com/throw-if-null/rat.api/actions/workflows/ci.yml) 
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=throw-if-null_rat.api&metric=coverage)](https://sonarcloud.io/dashboard?id=throw-if-null_rat.api) 
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=throw-if-null_rat.api&metric=alert_status)](https://sonarcloud.io/dashboard?id=throw-if-null_rat.api)

# Build

To build this project you can open it in Visual Studio and just build the solution, or run: `dotnet build` from `src` folder.

Note: If you don't have .net 5 SDK installed you can download it [here](https://dotnet.microsoft.com/download/dotnet/5.0)

# Test

Test can be ran either through Visual Studio, or by running: `dotnet test` from `src` folder

Note: By default test are running agains SqlLite database. Check '_Choosing the database_` section on how to switch to SqlServer database

## Chosing the database

Test suite supprots two database types:
1. SqlLite
2. SqlServer

### Using SqlLite

SqlLite is the default database

### Using SqlServer

To run test against SqlServer database use command: `dotnet test -e DATABASE_ENGINE=sqlserver`.  
Alternatively `DATABASE_ENGINE` variable can be added as an OS variable and then only `dotnet test` can be used.

# Run locally

In root folder there is a docker-compose file which will start Rat and SqlServer. Rat is configured to wait for SqlServer to become healthy before it starts.

If you don't want to run Rat from a Docker container you can run it from your local machine and to do so you will need to:
1. Navigate to: `\src\Rat.Api\bin\Debug\net5.0\` or `\src\Rat.Api\bin\Release\net5.0\`
2. Run: `dotnet Rat.Api.dll`


## How to create the database

Rat is using EF Core Migrations to manage the database lifecycle and for local development database will be created or updated automatically on application start.  
Do note, that this behavior is exclusive to local development and it is controlled with `ASPNETCORE_ENVIRONMENT` variable. To have it turned on `ASPNETCORE_ENVIRONMENT` needs to have value: `Development` (value set in docker-compose file).  

_Note_: When running from local maching `ASPNETCORE_ENVIRONMENT` variable can be set using `launchSetting.json` located in: `src\Rat.Api` folder.

## Healthchecks

Livenes probe can be reached at: `/health/live`
Readiness probe can be reached at: `/health/ready`

## Swagger

Rat uses [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to generate Api documentation.  
Swagger can be accessed at `/swagger`. 

## Authentication

Rat uses [Auth0](https://auth0.com/) as an authentication provided and each request excluding healthchecks expects an Authorization header with a Bearer token.
