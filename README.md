# README
This project can be used to monitor the health of Kubernetes Services. It is an .NET WebAPI which allows the user to check the availability of the services within a Kubernetes Cluster. The API contains endpoints for checking the health of the services and an endpoint to gather metrics about the API itself.

## Health endpoints

All health endpoints are documented using swagger and the route `/swagger/index.html` provides a detailed documentation of all these endpoints. The following endpoints are available
- `GET api/health/own-health` : Returns status code 200 if the API works
- `GET api/health/discovered-services-health` : Shows which previously discovered services are available at the moment
- `POST /api/health/service-discovery-urls` : Allows adding an url for service discovery and specifying an access token for the corresponding url
-  `POST /api/health/service-discovery-period-seconds`: Allows changing the intervall in which the health check is performed

## Metrics endpoint
The endpoint `/metrics` provides metrics about the API itself. The provided metrics match the standard Prometheus format, thus this endpoint can also be used to integrate Prometheus for monitoring purposes. 