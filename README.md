# Axle #

A simple service handling session management

## Prerequisites

Axle is secured by Ironclad (Bouncer). Before starting Axle you will also need a running instance of Ironclad.

Configure Ironclad by running following commands on console app of Ironclad.

### apis

The api is required to start the axle. Axle will use this api for introspection of reference token passed in the header when some client will call the secure endpoints of axle.

```
auth apis add axle_api secret -d "Session Management API (AXLE)" -c name -c role -c username -a axle_api -a axle_api:server -a axle_api:mobile -v
```

### clients

The client is required for sample single page application. Run the following command on ironclad console app to create the clients.

```
-- This is required for sample single page application
auth clients add website axle_spa -n "Axle Single Page Application" -c http://localhost:5013 -c http://127.0.0.1:5013 -r http://localhost:5013/callback.html -r http://127.0.0.1:5013/callback.html -l http://localhost:5013/index.html -l http://127.0.0.1:5013/index.html -a openid -a profile -a role -a axle_api -t Reference -g implicit -b -q -k -v

-- This is required only for swagger ui
auth clients add website axle_api_swagger -n "Swagger for Session Management (Axle)" -c http://localhost:5012 -c https://localhost:5120 -c http://127.0.0.1:5012 -c https://127.0.0.1:5120 -c http://axle.mt.svc.cluster.local:5012 -c https://axle.mt.svc.cluster.local:5120 -r http://axle.mt.svc.cluster.local:5012/swagger/oauth2-redirect.html -r https://axle.mt.svc.cluster.local:5120/swagger/oauth2-redirect.html -r http://localhost:5012/swagger/oauth2-redirect.html -r https://localhost:5120/swagger/oauth2-redirect.html -r http://127.0.0.1:5012/swagger/oauth2-redirect.html -r https://127.0.0.1:5120/swagger/oauth2-redirect.html  -a openid -a profile -a axle_api -a axle_api:server -a axle_api:mobile -q -b -k -v
```

## How to configure

All variables (Secrets/Settings) can be specified via ```appSettings.json``` file OR by environment variables / secrets.

### Secrets variables

This project requires specification of the [following user secrets](src/Axle/Program.cs?fileviewer=file-view-default#Program.cs-19) in order to function:

  | Parameter | Description
  | --- | --- |
  | Api-Authority / API_AUTHORITY | Authentication server url |
  | Api-Name / API_NAME | API name for this service on authentication server |
  | Api-Secret / API_SECRET | API secret for this service on authentication server |
  | mtCoreAccountsApiKey / MTCOREACCOUNTSAPIKEY | Api key for mt core accounts management service |
  | chestApiKey / CHEST_API_KEY | Api key for Chest service |
  | Require-Https / REQUIRE_HTTPS | Does https required by the authentication server (optional: default value is true) |
  | Swagger-Client-Id / SWAGGER_CLIENT_ID | Swagger client id for this service on authentication server (optional: default value is axle_api_swagger) |
  | Validate-Issuer-Name / VALIDATE_ISSUER_NAME | Should validate token issuer name when a secure endpoint is called (optional: default value is false) |
  | ConnectionStrings:Redis / REDIS_CONNECTIONSTRING | Connection string to Redis which should have a valid value |
  | ConnectionStrings:RabbitMQ / RABBITMQ_CONNECTIONSTRING | Connection string to RabbitMQ which should have a valid value |

As mentioned before, these secrets can also be set via ```appSettings.json``` file OR by environment variables, there is no strict requirement to provide them via secrets file

The secrets configuration mechanism differs when running the project directly or running inside a container. For detailed config specific to each platform, check section below.

### Settings variables

You can configure the ```appSettings.json``` replacing default values with desired ones for each variable. 

You can also add a file called ```appSettings.Custom.json``` with custom which will override the variables from ```appSettings.json``` or compose with it. 

Additionally you can add a file called ```appSettings.{environment}.json``` with environment specific configuration which will override the variables from ```appSettings.json``` and ```appSettings.Custom.json``` or compose with them.

These available variables are detailed below:

  | Parameter | Description |
  | --- | --- |
  | urls | Url that service will be exposed |
  | serilog:* | Serilog settings including output template, rolling file interval, file size limit and audit file configuration |
  | CorsOrigins:* | Cors origins configuration |
  | IntrospectionCache:Enabled | Whether the reference token introspection cache is enabled. Default true |
  | IntrospectionCache:DurationInSeconds | How long items in the cache should live for in seconds. Default 600 |
  | IntrospectionCache:ExpirationScanFrequencyInSeconds | Duration between each scan for expired items in the cache in seconds. Default 60 |
  | SessionConfig:TimeoutInSec | Session timeout in seconds, default value will be 300 seconds |
  | mtCoreAccountsMgmtServiceUrl | Url for MT Core accounts management service |
  | SecurityGroups | List of security settings with Group Name and Permissions allowed to it |
  | SecurityGroups:Name | Name of security group |
  | SecurityGroups:Permissions | List of permissions allowed to the security group |
  | ActivityPublisherSettings:ExchangeName | RabbitMQ exchange name for activities
  | ActivityPublisherSettings:IsDurable | RabbitMQ is durable value for activities publisher exchange |
  | chestUrl | Url for Chest service |
  | ConnectionStrings:RabbitMq | RabbitMQ connection string |
  | AuditSettings | List of audit settings, which will be used by [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs) to process requests and include data inside Logger scope. Whole section is optional |
  | AuditSettings:Enabled | Sets Audit as enabled = true or disabled = false. It's passed to [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs) |
  | AuditSettings:RolesToAudit | List of Roles to audit, that are passed to [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs) which checks if User roles match any of the roles provided. This is an optional parameter that when not provided or provided empty means ALL roles should be audited. |
  | AuditSettings:RoutesToAudit | List of Routes to audit that are passed to [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs) which checks if Request matches any of the routes provided.  This is an optional parameter that when not provided or provided empty means ALL routes should be audited. |

### Log specific configuration

- Logging mechanism in place uses Serilog with some enrichers to exposed better and more detailed logs (i.e [FromLogContext](https://github.com/serilog/serilog/wiki/Enrichment), [WithMachineName](https://github.com/serilog/serilog-enrichers-environment), [WithThreadId](https://github.com/serilog/serilog-enrichers-thread), [WithDemystifiedStackTraces](https://github.com/nblumhardt/serilog-enrichers-demystify)).

- There are three custom [middlewares](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1) injected in the application's request pipeline to enhance logs:
  
  1) [ExceptionHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/ExceptionHandlerMiddleware.cs) providing a global `Try/Catch` for unhandled [Exceptions](https://docs.microsoft.com/en-us/dotnet/api/system.exception?view=netcore-2.1)
  2) [LogHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/LogHandlerMiddleware.cs) providing a global `Logging` for [HttpRequests](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest?view=aspnetcore-2.1)
  3) [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs) which provides a global `Audit` control for [HttpRequests](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest?view=aspnetcore-2.1). It acts based on settings defined via `appsettings.json`.

  For more info please check [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) repository.

- Default configuration outputs log in three ways:

  1) [Console](https://github.com/serilog/serilog-sinks-console)
  2) General [File](https://github.com/serilog/serilog-sinks-file) without Audit specific logs, using serilog [Filters Expressions](https://github.com/serilog/serilog-filters-expressions)
  3) Audit [File](https://github.com/serilog/serilog-sinks-file) with Audit specific logs only, using serilog [Filters Expressions](https://github.com/serilog/serilog-filters-expressions) and Scope State value `ShouldAuditRequest` defined by [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs)
  
  The message format is the same for all:
  ```json
  {
    "Args": {
      "outputTemplate": "[{Timestamp:u}] [{Application}:{Version}:{Environment}] [{Level:u3}] [{RequestId}] [{CorrelationId}] [{ExceptionId}] {Message:lj} {NewLine}{Exception}"
    }
  }
  ```

  With following details:

  | Parameter | Description |
  | --- | --- |
  | Timestamp:u | Outputs current timestamp in UTC |
  | Application | Outputs the application name |
  | Version | Outputs the application version |
  | Version | Outputs the environment name from where application is running on, as defined by ASPNETCORE_ENVIRONMENT |
  | Level:u3 | Outputs the log message level as three-character uppercase (i.e ERR, INF, WRN, etc) |
  | RequestId | Request identifier generated by the framework or automatically the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | CorrelationId | Correlation identifier generated by the action (request header, hosted service action handler) or automatically by the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | ExceptionId | Exception identifier generated by the exception data or automatically by the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | Message:lj | Log message with embedded data in JSON format |
  | NewLine | Line break |
  | Exception | Exception message with stack trace |

  Some configuration options can be checked [here](https://github.com/serilog/serilog/wiki/Formatting-Output) and [here](https://github.com/serilog/serilog/wiki/Configuration-Basics)

### Platform specific configurations

All the configuration above can be set via ```appSettings.json```, but if you don't want to use it, below are some handful examples on how to do such based on where you are running it from

- If running the project from Visual Studio:  

  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from [secrets.json](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) like example below: 
  
  *NOTE*: File content should match the [expected required configuration](src/Axle/Program.cs?fileviewer=file-view-default#Program.cs-19).

  *NOTE* These secret values in example below are invalid
    ```json
    {
      "Api-Authority": "https://bouncer-dev.azurewebsite.net",
      "Api-Name": "axle_api",
      "Api-Secret": "secret",
      "ConnectionStrings": {
        "Redis": "<valid redis connection string>",
      },
      "Require-Https": true,
      "Swagger-Client-Id": "<swagger-client-id>",
      "Validate-Issuer-Name": false,
      "chestApiKey": "<api-key-for-chest-service>"
    }
    ```

- If you are running the project from the command line:  

  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from the command line in either Windows or Linux. You can set the secrets using the following command from within the ```src/Axle``` folder:
  
  *NOTE*: You may need to run a ```dotnet restore``` before you try these commands.

  *NOTE*: Secrets provided should match the [expected required configuration](src/Axle/Program.cs?fileviewer=file-view-default#Program.cs-19).

  *NOTE* These secret values in example below are invalid

    ```cmd
      dotnet user-secrets set "Api-Authority" "https://bouncer-dev.azurewebsite.net"
      dotnet user-secrets set "Api-Name" "axle_api"
      dotnet user-secrets set "Api-Secret" "secret"
      dotnet user-secrets set "ConnectionStrings:Redis" "<valid redis connection string>"
      dotnet user-secrets set "Require-Https" true
      dotnet user-secrets set "Swagger-Client-Id" "<swagger-client-id>"
      dotnet user-secrets set "Validate-Issuer-Name" false
      dotnet user-secrets set "chestApiKey": "<API key for Chest service>"
    ```

- If running the project inside a container:  
  
  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from the [environment variables](https://docs.docker.com/compose/environment-variables/#the-env_file-configuration-option) used to run the docker container.
  To do this you need to create an `.env` file in the `src/Docker` folder and enter key/value pairs in the format `KEY=VALUE` for each secret.

  *NOTE*: File content should match the [expected required configuration](src/Axle/Program.cs?fileviewer=file-view-default#Program.cs-19).

  *NOTE* These secret values in example below are invalid

    ```cmd
      API_AUTHORITY=https://bouncer-dev.azurewebsite.net
      API_NAME=axle_api
      API_SECRET=secret
      REDIS_CONNECTIONSTRING=<valid redis connection string>
      REQUIRE_HTTPS=true
      SWAGGER_CLIENT_ID=<swagger-client-id>
      VALIDATE_ISSUER_NAME=false
      CHEST_API_KEY=<Api key for Chest service>
    ```
	
### Add https enforcement for Axle

Set environment variables

  ```
    Kestrel__Certificates__Default__Path:</root/.aspnet/https/certFile.pfx>
    Kestrel__Certificates__Default__Password:<password>
  ```

In order to map path of certificate we need to add additional volume to docker-compose.yml file

  ```
    volumes:
      - ./https/:/root/.aspnet/https/
  ``` 
 
Update appsettings.Deployment.json file and mention the https port
 
 ``` 
 "urls": "https://*:443;"
 ```

Configuration of secrets.json file in order to use https

  ```json
    "Kestrel": {
      "EndPoints": {
        "HttpsInlineCertFile": {
        "Url": "https://*:443",
        "Certificate": {
            "Path": "<path to .pfx file>",
            "Password": "<certificate password>"
        }
      }
    }
  ```

    Example of Dockerfile
    ```
    FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
    WORKDIR /app
    EXPOSE 80

    FROM microsoft/dotnet:2.2-sdk AS build
    COPY . ./
    RUN cp NuGet.*onfig /usr/local/share/NuGet.Config 2>/dev/null || :
    WORKDIR /src/Axle
    RUN dotnet build -c Release -r linux-x64 -o /app

    FROM build AS publish
    RUN dotnet publish -c Release -r linux-x64 -o /app

    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app .
    ENTRYPOINT ["dotnet", "Axle.dll"]
    ```

## How to Use

A basic health check and version check can be performed by hitting this endpoint: `http://{axle-base-url}/api/isAlive`.
All endpoints are documented via Swagger which can be found under this URL: `http://{axle-base-url}/swagger`.

## How to Debug

### Using Visual Studio

Set the start-up project to ```Axle``` and launch it.
This will run the project directly using dotnet.exe. The application will listen on port 5012.

### Using Visual Studio Tools for Docker

Set the start-up project to ```docker-compose``` and launch it.
This will run the project inside a docker container running behind nginx. Nginx will listen on port 5012 and forward calls to the application.

### From the Command Line

Navigate to ```src/Axle``` folder and type ```dotnet run```.
You can also launch it with docker-compose command: Navigate to ```src/Docker``` and type ```docker-compose up```.
This will run the project directly using dotnet.exe without attaching the debugger. You will need to use your debugger of choice to attach to the dotnet.exe process.

# How to build docker image

This project contains a set of required files for a complete Docker image build, ready for usage. Only required input is a valid NuGet.config file with source for dependent libraries.

  Example of valid NuGet.config
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <packageSources>
      <add key="private-source" value="http://private-source.url/nuget/" />
    </packageSources>
  </configuration>
  ```

  With valid NuGet.config on your hands, you can simply copy it to workspace folder and run `./src/build`.

  Example of automation script
  ```cmd
  cd workspace/folder/
  cp original/path/for/NuGet.config .
  cd src/
  ./build
  ```