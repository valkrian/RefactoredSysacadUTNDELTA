# Guía de implementación (DTO + API + Angular) y ejecución local

Este documento explica el flujo completo implementado para el primer botón (Plan de Estudios), por qué se eligió esta arquitectura y cómo replicar el patrón para los demás botones. También incluye pasos detallados para levantar la API con base de datos en memoria y el frontend Angular.

---

## 1) ¿Qué se implementó y por qué?

### Objetivo del primer botón
El botón **Plan de Estudios** ahora:
- navega a una ruta real (`/plan`),
- consulta la API,
- muestra datos reales provenientes de una base en memoria,
- maneja estados de carga y error.

### Decisiones arquitectónicas (resumen)
- **DTOs en Application**: desacopla la API de las entidades EF y evita exponer modelos de dominio directamente.
- **Servicio de aplicación (IPlanService)**: separa la lógica de consulta y mapeo a DTO.
- **Infraestructura (PlanService)**: contiene la consulta EF y el mapeo a DTO.
- **DB en memoria + seed**: permite trabajar localmente sin Postgres ni migraciones, garantizando datos consistentes al iniciar.
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

## 3) Cómo implementar un DTO (paso a paso)

Ejemplo aplicado: Plan de Estudios.

1) Crear DTOs en Application:
   - Archivo: `api/Autogestion.Application/DTOs/PlanDto.cs`
   - Contenido:
     - `PlanDto`: datos principales del plan.
     - `PlanSubjectDto`: materias del plan.

2) Crear interfaz de servicio:
   - Archivo: `api/Autogestion.Application/Interfaces/IPlanService.cs`
   - Método: `GetPlanForStudentAsync(int studentId, CancellationToken ct)`

3) Implementar el servicio en Infrastructure:
   - Archivo: `api/Autogestion.Infrastructure/Services/PlanService.cs`
   - Consulta EF:
     - `Include(s => s.Plan).ThenInclude(p => p.Subjects)`
   - Mapeo a DTO.

4) Exponer endpoint en API:
   - Archivo: `api/Autogestion.Api/Program.cs`
   - Endpoint: `GET /students/me/plan`

---

## 4) Cómo replicar el patrón para otros botones

Para cada botón:

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
   - Agregar botón o link desde el dashboard.

3) Seed (si aplica)
   - Agregar datos mínimos para que la pantalla muestre contenido.

---

## 5) Cómo levantar la API en memoria (paso a paso)

Requisitos:
- .NET 8 SDK

Pasos:
1) Ir a la carpeta de la API:
```powershell
cd api/Autogestion.Api
```

2) Ejecutar:
```powershell
dotnet run
```

3) Abrir Swagger:
- http://localhost:5018/swagger

Notas:
- La base de datos en memoria se inicializa automáticamente.
- Los datos se crean en el seed al arrancar.

---

## 6) Cómo levantar Angular (paso a paso)

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
- Entrar a `/plan` o usar el botón "Plan de Estudios" del dashboard.

---

## 7) Preguntas frecuentes

**¿Por qué no se usan migraciones?**  
Porque el objetivo es trabajar local con datos controlados y sin depender de Postgres.

**¿Se puede volver a Postgres luego?**  
Sí. Basta con volver a `UseNpgsql(...)`, restaurar la cadena de conexión y usar migraciones.

---

## 8) Checklist rápido de replicación

- [ ] DTO creado en Application  
- [ ] Interfaz de servicio creada  
- [ ] Servicio EF en Infrastructure  
- [ ] Endpoint agregado en API  
- [ ] Modelo TS + servicio HTTP en Angular  
- [ ] Componente con estados (loading/error)  
- [ ] Ruta agregada  
- [ ] Botón/link conectado  
- [ ] Seed mínimo actualizado  
