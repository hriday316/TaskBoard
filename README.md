# TaskBoard

TaskBoard is scaffolded as a Clean Architecture application with:

- ASP.NET Core 10 / `net10.0`
- Angular 22
- Tailwind CSS 4
- PostgreSQL
- Entity Framework Core
- ASP.NET Core Identity with cookie authentication
- MediatR + CQRS-ready application layer

## Structure

```text
TaskBoard/
├── src/
│   ├── TaskBoard.Backend/
│   │   ├── TaskBoard.API/
│   │   ├── TaskBoard.Application/
│   │   ├── TaskBoard.Domain/
│   │   └── TaskBoard.Infrastructure/
│   └── TaskBoard.Frontend/
├── docker-compose.yml
└── TaskBoard.sln
```

## Run Locally

Start PostgreSQL:

```bash
docker compose up -d
```

Restore and build the backend:

```bash
dotnet restore TaskBoard.sln
dotnet build TaskBoard.sln
```

Run the API:

```bash
dotnet run --project src/TaskBoard.Backend/TaskBoard.API --launch-profile http
```

Run the frontend:

```bash
cd src/TaskBoard.Frontend
npm install
npm start
```

The API runs on `http://localhost:5000`, and Angular runs on `http://localhost:4200`.

 
