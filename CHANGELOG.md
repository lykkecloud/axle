## Unreleased

* LT-391: Enhancing documentation for service requirements, including more detailed descriptions
* LT-397: Enhancing logging with correct app version and with Lykke middleware and standards
* AXLE-38: Generate session id for currently logged user and invalidate session of the user. Set up Redis to persist session for the logged user

### Configuration changes:

  - Added ability to specify Secrets variables via `appSettings.json`

  - Added Cors origins configuration, it is important to pass the exact URLs of services or the Web application which will access Axle
 
```json
    "CorsOrigins": [ "http://localhost:5013", "http://nova.lykkecloud.com" ],
```

    - Added session timeout configuration. This configuration is optional if this timeout is not configured the default value will be 300 seconds

```json
	"SessionConfig": {
	    "TimeoutInSec": 300
	  }
```

All latest configuration changes that are used and working for dev environment can be found in ```appSettings.json```

