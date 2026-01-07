# REFACTOREDSYSACADUTNDELTA

Sysacad Actual

<img width="1908" height="511" alt="image" src="https://github.com/user-attachments/assets/075754a2-7b73-49c8-b01e-f783ebd681d0" />

Mi propuesta (inspirado en SIU Guarani)
<img width="1500" height="798" alt="image" src="https://github.com/user-attachments/assets/cfe9699f-b749-4071-b600-ed82c636d715" />


[![ultimo commit](https://img.shields.io/github/last-commit/valkrian/RefactoredSysacadUTNDELTA?style=flat-square)](https://github.com/valkrian/RefactoredSysacadUTNDELTA/commits)
[![licencia](https://img.shields.io/badge/licencia-MIT-informational?style=flat-square)](#licencia)
[![lenguajes](https://img.shields.io/github/languages/count/valkrian/RefactoredSysacadUTNDELTA?style=flat-square)](https://github.com/valkrian/RefactoredSysacadUTNDELTA)

Construido con herramientas y tecnologias:

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square)
![Angular](https://img.shields.io/badge/Angular-21-DD0031?style=flat-square)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?style=flat-square)

---

## Tabla de contenidos
- [Resumen](#resumen)
- [Funciones principales](#funciones-principales)
- [Arquitectura](#arquitectura)
- [Primeros pasos](#primeros-pasos)
  - [Requisitos](#requisitos)
  - [Instalacion](#instalacion)
  - [Configuracion](#configuracion)
  - [Ejecucion](#ejecucion)
- [Endpoints de la API](#endpoints-de-la-api)
- [Pruebas](#pruebas)
- [Roadmap](#roadmap)
- [Licencia](#licencia)

---

## Resumen
RefactoredSysacadUTNDELTA es un proyecto demo de portfolio que moderniza la autogestion de alumnos. Entrega una UI tipo administrativa, una API realista con reglas de negocio y una base PostgreSQL lista para uso local.
Es un pequeño aporte para poder darle vida al Sysacad. Me lo propuse al segundo de ver que necesita una actualizacion de UI. y de paso me sirvio para practicar backend.

## Funciones principales
- Plan de estudios por anio/cuatrimestre.
- Estado academico con resumen de aprobadas, regularizadas y pendientes.
- Examenes: mesas disponibles e inscripcion.
- Cursado: ofertas y alta de cursada.
- Perfil: ver y actualizar datos basicos.
- Reglas de correlatividad y validaciones de inscripcion.

## Arquitectura
Monolito modular con Clean Architecture:
- `Autogestion.Domain`: entidades y reglas de negocio (sin EF, sin HTTP).
- `Autogestion.Application`: DTOs, casos de uso e interfaces.
- `Autogestion.Infrastructure`: EF Core y casos de uso.
- `Autogestion.Api`: endpoints, DI, middleware y Swagger.

## Primeros pasos

### Requisitos
- .NET SDK 8
- Node.js LTS
- Docker (para PostgreSQL)

### Instalacion
```bash
git clone https://github.com/valkrian/RefactoredSysacadUTNDELTA.git
cd RefactoredSysacadUTNDELTA
```

### Configuracion
Crear o editar `backend/Autogestion.Api/appsettings.Development.json` (alineado con `docker-compose.yml`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=autogestion_db;Username=autogestion_user;Password=autogestion_password"
  },
  "SeedData": true
}
```

### Ejecucion
Backend + DB:
```bash
docker compose up -d
cd backend/Autogestion.Api
dotnet ef database update --project ../Autogestion.Infrastructure/Autogestion.Infrastructure.csproj --startup-project ./Autogestion.Api.csproj
dotnet run
```
Abrir Swagger en: `http://localhost:5018/swagger`

Frontend:
```bash
cd frontend
npm install
npm start
```
Abrir la app en: `http://localhost:4200`

## Endpoints de la API
Auth:
- `POST /auth/login`
- `GET /me`

Plan y estado:
- `GET /students/me/plan`
- `GET /students/me/status`

Examenes:
- `GET /exam-calls`
- `POST /students/me/exam-enrollments`
- `GET /students/me/exam-enrollments`

Cursado:
- `GET /course-offers`
- `POST /students/me/course-enrollments`
- `GET /students/me/course-enrollments`

Perfil:
- `GET /me/profile`
- `PUT /me/profile`
- `PUT /me/password`

## Pruebas
Backend:
```bash
dotnet test backend/Autogestion.Domain.Tests/Autogestion.Domain.Tests.csproj
```

Frontend:
```bash
cd frontend
npm test
```

## Roadmap
- Mejoras UX (loading/error/empty states).
- Validaciones avanzadas en inscripcion.
- Mas seeds de datos para escenarios realistas.

## Licencia
MIT, SIU Guarani, UTN DELTA, Sysacad
