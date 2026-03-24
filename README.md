# Gestor de Tickets de Soporte

Prueba técnica — Desarrollador Senior Full Stack

**Stack:** PostgreSQL 14 · .NET Core 6 Web API · Angular 16 + NgRx

---

## Requisitos

- Docker + Docker Compose  
  _O bien: .NET 6 SDK, Node 18, PostgreSQL 14 (ejecución local)_

---

## Levantar con Docker Compose (recomendado)

```bash
docker-compose up --build
```

| Servicio   | URL                          |
|------------|------------------------------|
| Frontend   | http://localhost:4200        |
| API        | http://localhost:5000        |
| Swagger UI | http://localhost:5000/swagger|
| PostgreSQL | localhost:5432               |

**Primera vez:** Docker inicializa la base de datos automáticamente con schema, funciones, índices y datos de prueba.

---

## Ejecución local (sin Docker)

### Base de datos

```bash
psql -U postgres -c "CREATE DATABASE support_tickets;"
psql -U postgres -d support_tickets -f database/01_schema.sql
psql -U postgres -d support_tickets -f database/02_functions.sql
psql -U postgres -d support_tickets -f database/03_indexes.sql
psql -U postgres -d support_tickets -f database/04_seed.sql
```

### Backend

```bash
cd backend/SupportTickets
dotnet run --project SupportTickets.API
# API disponible en http://localhost:5000
```

> Ajusta el connection string en `SupportTickets.API/appsettings.json` si es necesario.

### Frontend

```bash
cd frontend/support-tickets-app
npm install
npm start
# App disponible en http://localhost:4200
```

---

## Estructura del proyecto

```
support-tickets/
├── database/
│   ├── 01_schema.sql        # Tablas + historial
│   ├── 02_functions.sql     # buscar_tickets() PL/pgSQL
│   ├── 03_indexes.sql       # Índices comentados
│   └── 04_seed.sql          # 8 usuarios, 4 categorías, 30 tickets, 60 comentarios
├── backend/SupportTickets/
│   ├── SupportTickets.Core/     # Modelos, DTOs, Interfaces
│   ├── SupportTickets.Data/     # Repositorios (Dapper + Npgsql)
│   ├── SupportTickets.Services/ # Lógica de negocio
│   └── SupportTickets.API/      # Controllers, DI, Swagger
├── frontend/support-tickets-app/
│   └── src/app/
│       ├── core/            # Modelos TS + TicketsService
│       └── features/tickets/
│           ├── store/       # NgRx: state, actions, reducer, effects, selectors
│           └── components/  # ticket-list, ticket-detail, ticket-create
└── docker-compose.yml
```

---

## Endpoints de la API

| Verbo  | Ruta                              | Descripción                    |
|--------|-----------------------------------|--------------------------------|
| GET    | /api/tickets                      | Búsqueda paginada con filtros  |
| GET    | /api/tickets/{id}                 | Detalle con comentarios e historial |
| POST   | /api/tickets                      | Crear ticket                   |
| PATCH  | /api/tickets/{id}/estatus         | Cambiar estatus                |
| PATCH  | /api/tickets/{id}/asignar         | Asignar agente                 |
| POST   | /api/tickets/{id}/comentarios     | Agregar comentario             |
| GET    | /api/catalogos/{tipo}             | categorias / agentes / usuarios|

### Simulación de usuario
El usuario activo se pasa por header: `X-Usuario-Id: 1`  
Por defecto el sistema usa el usuario ID 1 si no se especifica.

---

## Reglas de negocio

1. **Regla 1:** Un ticket no puede pasar a `en_progreso` sin agente asignado.  
   → HTTP 422 con mensaje descriptivo.

2. **Regla 2:** Un ticket `cerrado` no acepta nuevos comentarios.  
   → HTTP 422. El frontend muestra el mensaje y deshabilita el formulario.

3. **Regla 3:** Solo se puede asignar un usuario con rol `agente`.  
   → HTTP 422 si se intenta asignar un usuario regular.

---

## Puntos extra implementados

- ✅ **+3 pts — Docker Compose:** Levanta PostgreSQL, API y Angular con un solo comando.
- ✅ **+2 pts — Historial de cambios:** Tabla `historial_tickets` que registra cada cambio de estatus y asignación. Visible en la pantalla de detalle del ticket.

---

## Decisiones técnicas

- **Dapper** sobre EF Core: mayor control del SQL, mejor integración con la función PL/pgSQL `buscar_tickets()`.
- **NgRx Effects con switchMap** para lecturas (cancelables) y **concatMap** para escrituras (preserva orden).
- **Filtros en el store:** al navegar al detalle y regresar, los filtros y la página se preservan automáticamente.
- **Capas separadas** (Core / Data / Services / API): las reglas de negocio viven en `TicketService`, los controllers son solo ruteadores HTTP.
- **Historial** registrado en el servicio al cambiar estatus o asignar agente, sin lógica extra en el repositorio.
