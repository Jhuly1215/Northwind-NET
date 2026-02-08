# Northwind Microservices (.NET + MySQL)

Repositorio de **microservicios con .NET (net9.0)** usando la base de datos de ejemplo **Northwind** en **MySQL 8** y documentación vía **Swagger/OpenAPI**.

## ¿Qué incluye?

- **MySQL (Northwind)** levantado con Docker y scripts de inicialización
- **Adminer** para inspeccionar la base de datos desde el navegador
- **CatalogService.Api** (productos) con Swagger
- **OrdersService.Api** (órdenes) con Swagger y versionado de API (v1/v2)
- Desarrollo con `dotnet watch` dentro de contenedores (hot reload)

## Estructura del repo

- `infra/northwind-infra/mysql/init/`
  - `01-northwind-schema.sql` crea el esquema/tablas
  - `02-northwind-data.sql` carga datos de ejemplo
- `src/CatalogService/CatalogService.Api/`
  - API de catálogo (productos)
- `src/OrdersService/OrdersService.Api/`
  - API de órdenes (orders, order_details, customers, etc.)
- `docker-compose.yml`
  - Orquestación de MySQL + Adminer + APIs

## Requisitos

- Docker + Docker Compose
- (Opcional) .NET SDK 9 para correr fuera de Docker

## Configuración (variables de entorno)

Crear archivo `.env` en la raíz (base en `.env.example`).

Ejemplo típico:

```env
ASPNETCORE_ENVIRONMENT=Development

MYSQL_ROOT_PASSWORD=root
MYSQL_DATABASE=northwind
MYSQL_USER=app
MYSQL_PASSWORD=app123
MYSQL_PORT=3306

ADMINER_PORT=8081

CATALOG_API_PORT=5000
ORDERS_API_PORT=5001
```
## Levantar todo
En la raíz del repo:

```
docker compose up --build
```

Para bajar contenedores:

```
docker compose down
```

---
## URLs útiles

- **Adminer**: `http://localhost:<ADMINER_PORT>`
  - Server/Host: `mysql`
  - User: `${MYSQL_USER}`
  - Password: `${MYSQL_PASSWORD}`
  - Database: `${MYSQL_DATABASE}`
  
- **Catalog Swagger**: `http://localhost:<CATALOG_API_PORT>/swagger`
- **Orders Swagger**: `http://localhost:<ORDERS_API_PORT>/swagger`

---

## Versionado de Orders API

El servicio `OrdersService.Api` expone rutas versionadas:

- `GET /api/v1/orders`
- `GET /api/v1/orders/{id}`
- `POST /api/v1/orders`
- `PUT /api/v1/orders/{id}`
- `PATCH /api/v1/orders/{id}/status`
- `DELETE /api/v1/orders/{id}`

Y un ejemplo adicional en v2:

- `GET /api/v2/orders` (incluye resumen con total y nombre del cliente)
---

## Healthcheck

Cada API expone:

- `GET /health`
(Útil para verificar que el servicio está arriba y que la DB responde.)
---

