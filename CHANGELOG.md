## 2.11.0 (March 8, 2019)

* LT-907: Removing private nuget sources from Nuget.config
* AXLE-56: removed account id check when creating session object
* AXLE-53: fixed nuget client
* AXLE-31: Publish session activities in RabbitMQ

### Axle Service

#### Configuration changes:

-Added roles and permissions;

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
      "Name": "read-only01",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "credit01",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "backoffice-trading01",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
      ]
    },
    {
      "Name": "backoffice-administration01",
      "Permissions": [
        "start-session-without-account",
        "on-behalf-account-selection"
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

- Added reference to mt core service and MT core account management url is requiered;

```json
  "mtCoreAccountsMgmtServiceUrl": "mtcore account url",
```

- Added reference to chest service and chest url is requiered;

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

Axle client which can be used by otehr services in order to call endpoint from axle service

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

