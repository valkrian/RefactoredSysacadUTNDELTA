# Guia de implementacion (DTO + API + Angular) y ejecucion local

Este documento explica el flujo completo implementado para el primer boton (Plan de Estudios), por que se eligio esta arquitectura y como replicar el patron para los demas botones. Tambien incluye pasos detallados para levantar la API con PostgreSQL local (migraciones) y el frontend Angular.

---

## 1) Que se implemento y por que?

### Objetivo del primer boton
El boton **Plan de Estudios** ahora:
- navega a una ruta real (`/plan`),
- consulta la API,
- muestra datos reales provenientes de PostgreSQL local,
- maneja estados de carga y error.

### Decisiones arquitectonicas (resumen)
- **DTOs en Application**: desacopla la API de las entidades EF y evita exponer modelos de dominio directamente.
- **Servicio de aplicacion (IPlanService)**: separa la logica de consulta y mapeo a DTO.
- **Infraestructura (PlanService)**: contiene la consulta EF y el mapeo a DTO.
- **PostgreSQL + migraciones + seed opcional**: permite trabajar con datos reales y un seed controlado en Development.
- **Angular con servicios**: las pantallas consumen datos reales usando un servicio HTTP simple.

---

## 2) Estructura recomendada (resumen)

Backend (.NET):
- `Autogestion.Domain/Entities`
- `Autogestion.Application/DTOs`
- `Autogestion.Application/Interfaces`
- `Autogestion.Infrastructure/Data`
- `Autogestion.Infrastructure/Data/Seed`
- `Autogestion.Infrastructure/Services`
- `Autogestion.Api` (endpoints)

Frontend (Angular):
- `src/app/components/<modulo>`
- `src/app/services`
- `src/app/models`

---

## 3) Como implementar un DTO (paso a paso)

Ejemplo aplicado: Plan de Estudios.

1) Crear DTOs en Application:
   - Archivo: `api/Autogestion.Application/DTOs/PlanDto.cs`
   - Contenido:
     - `PlanDto`: datos principales del plan.
     - `PlanSubjectDto`: materias del plan.

2) Crear interfaz de servicio:
   - Archivo: `api/Autogestion.Application/Interfaces/IPlanService.cs`
   - Metodo: `GetPlanForStudentAsync(int studentId, CancellationToken ct)`

3) Implementar el servicio en Infrastructure:
   - Archivo: `api/Autogestion.Infrastructure/Services/PlanService.cs`
   - Consulta EF:
     - `Include(s => s.Plan).ThenInclude(p => p.Subjects)`
   - Mapeo a DTO.

4) Exponer endpoint en API:
   - Archivo: `api/Autogestion.Api/Program.cs`
   - Endpoint: `GET /students/me/plan`

---

## 4) Como replicar el patron para otros botones

Para cada boton:

1) Backend
   - Crear DTOs nuevos.
   - Definir interfaz de servicio.
   - Implementar el servicio en Infrastructure (consulta + mapeo).
   - Exponer endpoint en `Program.cs`.

2) Frontend
   - Crear modelo TypeScript en `models`.
   - Crear servicio HTTP en `services`.
   - Crear componente en `components/<modulo>`.
   - Agregar ruta en `app.routes.ts`.
   - Agregar boton o link desde el dashboard.

3) Seed (si aplica)
   - Agregar datos minimos para que la pantalla muestre contenido.

---

## 5) Como levantar la API con PostgreSQL local (paso a paso)

Requisitos:
- .NET 8 SDK
- PostgreSQL local (o Docker)

Pasos:
1) Ir a la carpeta de la API:
```powershell
cd api/Autogestion.Api
```

2) Configurar la cadena de conexion en `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Bd_sysacad;Username=postgres;Password=admin"
  },
  "SeedData": true
}
```

3) Aplicar migraciones:
```powershell
dotnet ef database update --project ../Autogestion.Infrastructure/Autogestion.Infrastructure.csproj --startup-project ./Autogestion.Api.csproj
```

4) Ejecutar:
```powershell
dotnet run
```

5) Abrir Swagger:
- http://localhost:5018/swagger

Notas:
- El seed solo corre en Development si `SeedData` es `true`.
- Para desactivar el seed, cambiar `SeedData` a `false`.

---

## 6) Como levantar Angular (paso a paso)

Requisitos:
- Node.js LTS

Pasos:
1) Ir al frontend:
```powershell
cd apps/web-angular
```

2) Instalar dependencias:
```powershell
npm install
```

3) Levantar la app:
```powershell
npm start
```

4) Abrir:
- http://localhost:4200

5) Probar:
- Entrar a `/plan` o usar el boton "Plan de Estudios" del dashboard.

---

## 7) Preguntas frecuentes

**Por que usar migraciones?**  
Porque la base se crea de forma reproducible y el esquema queda versionado.

**Como desactivo el seed?**  
Poner `SeedData: false` en `appsettings.Development.json`.

---

## 8) Checklist rapido de replicacion

- [ ] DTO creado en Application  
- [ ] Interfaz de servicio creada  
- [ ] Servicio EF en Infrastructure  
- [ ] Endpoint agregado en API  
- [ ] Modelo TS + servicio HTTP en Angular  
- [ ] Componente con estados (loading/error)  
- [ ] Ruta agregada  
- [ ] Boton/link conectado  
- [ ] Seed minimo actualizado  
