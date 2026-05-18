# Vega — Agricultural Pharmacy E-Commerce

Vega is a full-stack e-commerce platform for an agricultural pharmacy. The
backend is split into independently-deployable ASP.NET Core microservices and
the frontend is a React + TypeScript SPA.

The UX takes inspiration from docmorris.de — a calm, trustworthy pharmacy look
with green accents, large product imagery, and a clear "add to cart" flow.

## Architecture

```
+----------------+        +-----------------+
|  React (Vite)  | <----> |  User Service   |  /auth, /users
|  TypeScript    |        +-----------------+
|                |        +-----------------+
|                | <----> | Product Service |  /products
|                |        +-----------------+
|                |        +-----------------+
|                | <----> |  Cart Service   |  /cart
|                |        +-----------------+
|                |        +-----------------+
|                | <----> |  Order Service  |  /orders --> Payment Service
+----------------+        +-----------------+
```

Each service owns its own PostgreSQL schema. Services communicate over HTTP.
The frontend talks to every service through REST; JWTs issued by the User
Service are attached to authenticated requests.

## Services

| Service  | Port | Folder                           |
| -------- | ---- | -------------------------------- |
| User     | 5001 | `services/Vega.UserService`      |
| Product  | 5002 | `services/Vega.ProductService`   |
| Cart     | 5003 | `services/Vega.CartService`      |
| Order    | 5004 | `services/Vega.OrderService`     |
| Payment  | 5005 | `services/Vega.PaymentService`   |
| Frontend | 5173 | `frontend/`                      |

## Running locally

Each service expects a PostgreSQL connection string in `appsettings.json` or
the `ConnectionStrings__Default` env var. Bring up the DB, then:

```
dotnet run --project services/Vega.UserService
dotnet run --project services/Vega.ProductService
dotnet run --project services/Vega.CartService
dotnet run --project services/Vega.OrderService
dotnet run --project services/Vega.PaymentService
```

Frontend:

```
cd frontend
npm install
npm run dev
```

## Tech

- ASP.NET Core 8 Web API, EF Core, PostgreSQL
- JWT bearer auth (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- React 18 + TypeScript, React Router, Axios, Vite
