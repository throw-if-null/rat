# Rat
[![Rat API - CI/CD](https://github.com/throw-if-null/rat/actions/workflows/ci.yml/badge.svg)](https://github.com/throw-if-null/rat/actions/workflows/ci.yml)[![Rat API - Documentation](https://github.com/throw-if-null/rat/actions/workflows/redoc.yml/badge.svg)](https://github.com/throw-if-null/rat/actions/workflows/redoc.yml)
[![Rat DB - Golden Image](https://github.com/throw-if-null/rat/actions/workflows/sqlserver.yml/badge.svg)](https://github.com/throw-if-null/rat/actions/workflows/sqlserver.yml)

# :rat: Rat

The :rat: is configuration manager mediator that is capable of importing data from different sources.  
Currently: _JSON file_, _MongoDb_ and/or _evnironment variables_ with goal to extend that support towrads: _Consul_, _ETCD_, _MS SQL_ other :rat: instance(s) and probable some other sources.

# Motivation
Maintaing and handling configuration is one of more tedious and at some point complex problems that you need to resolve when building your application(s). Even if you have just one application the amount of configuration can be huge, or it can be just couple of keys and a connection string that after a few years become a page that requires a documentation!  
Now there are many ways to store your configuration entries if the file is becoming to cumbersome, you could just store it in the database and be done with it, or go with enterprise solutions like [Consul](https://www.consul.io/docs/agent/kv.html) or [ETCD](https://etcd.io/). [Consul](https://www.consul.io/docs/agent/kv.html) even offers you a very nice [UI](https://learn.hashicorp.com/consul/getting-started/ui) for managing your configuration keys.  
So, why yet another configuration manager?  
Because of need for something simple - yet very extendable.  
Something that can have a lifecycle of a "simple" API.  
Something that is capable of importing configurations from different sources: files, databases, KV stores or even other APIs.  
Something that can fit into an already matured environment that has lots of different services: legacy, old and new that are going to be built.  
Something that will not require extra work, or huge refactoring in order to become a single source of truth.  

So, that's how the idea for :rat: came to life. 

# Build

To build this project you can open it in Visual Studio and just build the solution, or run: `dotnet build` from `src` folder.

Note: If you don't have .net 5 SDK installed you can download it [here](https://dotnet.microsoft.com/download/dotnet/5.0)

# Test

Test can be ran either through Visual Studio, or by running: `dotnet test` from `src` folder

Note: By default test are running agains SqlLite database. Check '_Choosing the database_` section on how to switch to PostGre/SqlServer database

## Chosing the database

Test suite supprots two database types:
1. SqlLite
2. PostGre/SqlServer

### Using SqlLite

SqlLite is the default database

### Using PostGre/SqlServer

To run test against SqlServer database use command: `dotnet test -e DATABASE_ENGINE=sqlserver/postgre`.  
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
