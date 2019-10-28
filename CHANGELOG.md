## 2.14.4 (October 28, 2019)

* LT-1720: Add socket connection event for session termination

## 2.14.3 (October 22, 2019)

* LT-1717: simplify session get

## 2.14.2 (October 15, 2019)

* LT-1707: Cannot find a session for investor

## 2.14.1 (October 07, 2019)

* LT-1635: [UAT-performance] Axle Timeouts issue with Redis

## 2.14.0 (July 08, 2019)

* LT-1541: Update licenses headers and add LICENSE  file

## 2.13.2 (June 14, 2019)

* AXLE-61: Modified storage to keep investor sessions per account
* AXLE-62: Added hub method for logging out
* AXLE-59: Allow only one tab to be open by a given user
* LT-1378: Fix on behalf activities not generating when support session is started or terminated
* MTC-824: Add optional ApiKey to client generation

### Configuration changes
  - Added variables for MT Core services API keys. If any variable is left unset or empty API call will be performed without API key.
```none
  mtCoreAccountsApiKey / MTCOREACCOUNTSAPIKEY
```

## 2.13.1 (May 15, 2019)

* LT-1320: Enable Audit logs

### Configuration changes
  - Added following section for Audit log settings. It enables Audit logs and sets which roles/routes will be tracked by [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs).
  ```json
  {
    "AuditSettings":{
      "Enabled": true,
      "RolesToAudit": [
        "customer-care",
        "credit",
        "backoffice-trading",
        "backoffice-administration",
        "role1",
        "role2",
        "role3",
        "role4",
        "role5",
        "role6"
      ],
      "RoutesToAudit": [
        { "Method": "DELETE", "Template": "/api/Sessions" }
      ]
    },
  }
  ```
  - Changed following section of Serilog config which filters out Audit specific logs from general log file.
  ```json
  {
    "Name": "Logger",
    "Args": {
      "configureLogger": {
        "filter": [{ 
          "Name": "ByExcluding", 
          "Args": { "expression": "Contains(SourceContext, 'AuditHandlerMiddleware')" }
        }],
        "writeTo": [
          {
            "Name": "File",
            "Args": {
              "outputTemplate": "[{Timestamp:u}] [{Application}:{Version}:{Environment}] [{Level:u3}] [{RequestId}] [{CorrelationId}] [{ExceptionId}] {Message:lj} {NewLine}{Exception}",
              "path": "logs/Axle/Axle-developer.log",
              "rollingInterval": "Day",
              "fileSizeLimitBytes": null
            }
          }
        ]
      }
    }
  }
  ```
  - Added following section to Serilog config which specifies different place for Audit logs file. It pipes data using filter based on scope variables (`SourceContext`, `ShouldAuditRequest`) defined by [AuditHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/AuditHandlerMiddleware.cs). This variable `ShouldAuditRequest` is calculated based on AuditSettings section.
  ```json
  {
    "Name": "Logger",
    "Args": {
      "configureLogger": {
        "filter": [{ 
          "Name": "ByIncludingOnly", 
          "Args": { "expression": "Contains(SourceContext, 'AuditHandlerMiddleware') and ShouldAuditRequest = True" }
        }],
        "writeTo": [
          {
            "Name": "File",
            "Args": {
              "outputTemplate": "[{Timestamp:u}] [{Application}:{Version}:{Environment}] [{Level:u3}] [{RequestId}] [{CorrelationId}] [{ExceptionId}] {Message:lj} {NewLine}",
              "path": "logs/Axle/Axle-audit-developer.log",
              "rollingInterval": "Day",
              "fileSizeLimitBytes": null
            }
          }
        ]
      }
    }
  }
  ```

## 2.13.0 (April 10, 2019)

* LT-1240: Update Licenses
* LT-1264: Enabled introspection cache

### Configuration changes

  - Added configuration for reference token introspection cache

```json
  "IntrospectionCache": {
    "Enabled": true,
    "DurationInSeconds": 600,
    "ExpirationScanFrequencyInSeconds": 60
  }
```

## 2.12.0 (March 27, 2019)

* CONGA-4: Implement Login Error Messages
* LT-1120: Fixed warnings for packages version and misusage, which also led to app crash in first web request
* LT-1086: Exposed endpoint to manually publish login activity (only for mobile clients)
* LT-1193: Validate api authority url and tested api/isAlive endpoint. Thrown proper error message on failure
* LT-1210: Removed wrong error message when secrets provided from appsettings.json instead of user secrets

### Lykke.Snow.Common.Startup

Lykke.Snow.Common.Startup was updated and a new nuget version is published (Version 1.2.6)

Missing secret wrong error message is removed and a few more improvements made while adding environment variables and secrets

### Configuration changes:

  - Added `axle_api:mobile` scope to `axle_api`. Run following commands on bouncer console to remove and recreate `axle_api` api

```cmd
auth apis remove axle_api -v

auth apis add axle_api secret -d "Session Management API (AXLE)" -c name -c role -c username -a axle_api -a axle_api:server -a axle_api:mobile -v
```

## 2.11.0 (March 8, 2019)

* LT-907: Removing private nuget sources from Nuget.config
* AXLE-56: removed account id check when creating session object
* AXLE-53: fixed nuget client
* AXLE-31: Publish session activities in RabbitMQ
 
### Axle Service

#### Configuration changes:

- Added roles and permissions;

```json
 "SecurityGroups": [
    {
      "Name": "customer-care",
      "Permissions": [
        "cancel-session",
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "read-only",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "credit",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "backoffice-trading",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "backoffice-administration",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "consors-user-admin",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role1",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role2",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role3",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role4",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role5",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "role6",
      "Permissions": [
        "start-session-without-account"
      ]
    },
    {
      "Name": "bnp-user-admin",
      "Permissions": [
        "start-session-without-account"
      ]
    }
  ]
```

-Added activity publisher settings, new exchange for login events is required;

```json
"ActivityPublisherSettings": {
    "ExchangeName": "lykke.axle.activities",
    "IsDurable": true
  }
```

- Added reference to mt core service and MT core account management url is required;

```json
  "mtCoreAccountsMgmtServiceUrl": "mtcore account url",
```

- Added reference to chest service and chest url is required;

```json
  "chestUrl": "chest url",
```

- No need to specify rabbit mq connection string on exchange configuration.
  Instead we have ConnectionStrings:RabbitMq
  
```json
"ConnectionStrings": {
	 "RabbitMq": "rabbit mq connection string"
	}
```

RabbitMq Connection string can be passed to secrets as well 

```
ConnectionStrings:RabbitMQ / RABBITMQ_CONNECTIONSTRING | Connection string to RabbitMQ which should have a valid value 
```

#### Secrets variables

  | ConnectionStrings:RabbitMq / RABBITMQ_CONNECTIONSTRING | Connection string to RabbitMq server |

All latest configuration changes that are used and working for dev environment can be found in ```appSettings.json```

### Axle.Client

Axle client which can be used by other services in order to call endpoint from axle service

### Axle.Contracts

A nuget package used for activities models


## 2.10.0 (February 18, 2019)

* LT-391: Enhancing documentation for service requirements, including more detailed descriptions
* LT-397: Enhancing logging with correct app version and with Lykke middleware and standards
* LT-907: Removing private nuget sources from Nuget.config
* AXLE-38: Generate session id for currently logged user and invalidate session of the user. Set up Redis to persist session for the logged user

### Configuration changes:

  - Added ability to specify Secrets variables via `appSettings.json`
  - Added ability to read global Nuget.config file injected in workspace folder and apply during docker image build
  - Added Cors origins configuration, it is important to pass the exact URLs of services or the Web application which will access Axle
 
  ```json
    "CorsOrigins": [ 
      "http://localhost:5013", 
      "http://nova.lykkecloud.com" 
    ],
  ```

  - Added session timeout configuration. This configuration is optional if this timeout is not configured the default value will be 300 seconds

  ```json
    "SessionConfig": {
      "TimeoutInSec": 300
    }
  ```

  All latest configuration changes that are used and working for dev environment can be found in ```appSettings.json```

