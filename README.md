# desafio-net
Desafio: Desenvolvimento de API RESTful

## Technologies implemented

* Asp.Net Core
* Entity Framework Core
* Swagger
* Fluent Validation
* Identity
* Docker
* PostgreSQL
* MiniProfiler

## Architecture

* Layer architecture
* S.O.L.I.D. principles
* Clean Code
* Repository Pattern
* Notification Pattern
* Domain Driven Design

## How to use it

To use the API, login is required.
On the route: /api/v1/auth/login

[Swagger](https://localhost:44302/swagger).
```
{
  "email": "user@example.com",
  "password": "string"
}
```

If you don't have login.
On the route: /api/v1/auth/register

```
{
  "email": "user@example.com",
  "password": "string",
  "confirmPassword": "string",
}
```

### MiniProfiler

Routes to use:
[Profiler index](https://localhost:44302/profiler/results-index).
[Profiler list](https://localhost:44302/profiler/results-list).
[Profiler results](https://localhost:44302/profiler/results).
