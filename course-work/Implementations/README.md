# CineBook – Система за резервации на кино прожекции

**Факултетен номер:** 2401322032

---

## Описание

CineBook е уеб приложение за управление и резервация на места за кино прожекции. Системата предоставя REST API и Blazor Server потребителски интерфейс. Потребителите могат да разглеждат филми и прожекции и да резервират места чрез интерактивна схема на залата. Администраторите управляват филми, зали и прожекции след вход с JWT authentication.

---

## Технологии

- ASP.NET Core 9 (REST API)
- Blazor Web App (Interactive Server)
- PostgreSQL + Entity Framework Core 9
- JWT Bearer authentication
- BCrypt.Net-Next

---

## Предварителни изисквания

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 14+](https://www.postgresql.org/download/) или Docker
- EF Core Tools: `dotnet tool install --global dotnet-ef`

---

## Инсталация и стартиране

### 1. Стартиране на PostgreSQL с Docker

```bash
docker run -d --name cinebook-postgres \
  -e POSTGRES_DB=CineBook \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=supersecret \
  -p 5433:5432 postgres:16
```

### 2. Конфигурация

`CineBook/CineBook.Api/appsettings.json` е предварително конфигуриран за горния контейнер.

### 3. Стартиране на API

```bash
cd CineBook/CineBook.Api
dotnet run
```

API: `https://localhost:7185` · Swagger: `https://localhost:7185/swagger`

Миграциите и началните данни се прилагат автоматично при първото стартиране.

### 4. Стартиране на Blazor

В нов терминал:

```bash
cd CineBook/CineBook.Blazor
dotnet run
```

Приложение: `https://localhost:7170`

---

## Администраторски акаунт

| Имейл | Парола |
|-------|--------|
| `admin@cinebook.bg` | `Admin123!` |