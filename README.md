# Axle
A simple service handling session management

### apis

The api is required to start the axle. Axle will use this api for instrospection of reference token passed in the header when some client will call the secure endpoints of axle.

```
auth apis add axle_api secret -d "Session Management API (AXLE)" -c name -c role -c username -a axle_api -a axle_api:server -v
```

### clients

The client is required for sample single page application. Run the following command on ironclad console app to create the clients.

```
-- This is required for sample single page application
auth clients add website axle_spa -n "Axle Single Page Application" -c http://localhost:5013 -c http://127.0.0.1:5013 -r http://localhost:5013/callback.html -r http://127.0.0.1:5013/callback.html -l http://localhost:5013/index.html -l http://127.0.0.1:5013/index.html -a openid -a profile -a role -a axle_api -t Reference -g implicit -b -q -k -v

-- This is required only for swagger ui
auth clients add website axle_api_swagger -n "Swagger for Session Management (Axle)" -c http://localhost:5012 -c https://localhost:5120 -c http://127.0.0.1:5012 -c https://127.0.0.1:5120 -c http://axle.mt.svc.cluster.local:5012 -c https://axle.mt.svc.cluster.local:5120 -r http://axle.mt.svc.cluster.local:5012/swagger/oauth2-redirect.html -r https://axle.mt.svc.cluster.local:5120/swagger/oauth2-redirect.html -r http://localhost:5012/swagger/oauth2-redirect.html -r https://localhost:5120/swagger/oauth2-redirect.html -r http://127.0.0.1:5012/swagger/oauth2-redirect.html -r https://127.0.0.1:5120/swagger/oauth2-redirect.html  -a openid -a profile -a axle_api -q -b -k -v
```