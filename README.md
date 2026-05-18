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

Each service owns its own PostgreSQL schema (`users`, `products`, `carts`,
`orders`) inside a shared `vega` database — services never read another
service's schema, only its HTTP API. Services communicate over HTTP.
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

Bring up Postgres (a single `vega` database; each service creates its own
schema on first migrate):

```
docker compose up -d
```

Run each service in its own shell:

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
cp .env.example .env
npm install
npm run dev
```

Open http://localhost:5173.

### Notes

- The User Service signs JWTs that the other services validate, so they all
  share the `Jwt:Issuer/Audience/Key` config block. In production, that key
  belongs in a secret store, not in `appsettings.json`.
- The Product Service seeds a small inventory on first boot so the storefront
  has something to render.
- The Payment Service is a sandbox: it sleeps for a moment and returns
  `Paid` for most orders, `Failed` for a small random slice plus any
  suspiciously round high-value order.

## Tech

- ASP.NET Core 8 Web API, EF Core, PostgreSQL
- JWT bearer auth (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- React 18 + TypeScript, React Router, Axios, Vite
