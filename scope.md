# scope.md — Modernización Autogestión Alumnos (Demo Portfolio)

## 1) Objetivo
Modernizar la experiencia de “Autogestión Alumnos” (legacy ASP) en un **demo funcional** con:
- UI moderna tipo sistema administrativo (Bootstrap)
- Backend realista con reglas de negocio (correlatividades intermedias)
- Base de datos **ficticia** en **PostgreSQL**
- Historial de trabajo visible: **commits diarios** + **push mensual** a `develop` y luego a `main`

> Nota: datos 100% inventados. No replica sistemas reales, solo inspiración funcional.

---

## 2) Stack y ejecución local
### Frontend
- Angular (standalone) + Bootstrap

### Backend
- .NET 8 Web API
- Swagger/OpenAPI

### DB
- PostgreSQL (Docker)

### Dev runtime
- Docker: **solo** PostgreSQL
- API y Web: corren local (dotnet + ng)

---

## 3) Estilo de arquitectura
### Monolito modular por capas (Clean Architecture liviana)
**API (por proyectos):**
- `Autogestion.Domain`  
  Entidades + reglas puras (sin EF, sin HTTP)
- `Autogestion.Application`  
  Casos de uso, DTOs, interfaces (ports)
- `Autogestion.Infrastructure`  
  EF Core, repositorios, auth, persistencia Postgres
- `Autogestion.Api`  
  Controllers, DI, middleware, Swagger, policies

**Angular:** SPA separada consumiendo API REST.

---

## 4) Módulos MVP (v1)
1) **Materias del plan** (visualización por año/cuatrimestre, estado)
2) **Estado académico** (aprobadas / regularizadas / pendientes, promedios demo)
3) **Exámenes + Inscripción a examen**
4) **Cursado + Inscripción a cursado**

Fuera de scope (por ahora):
- Certificados, encuestas, avisos, cambio de contraseña, boleto educativo.

---

## 5) Reglas de negocio 
### Correlatividades 
- Para **cursar**: requiere materias **regularizadas o aprobadas** (según regla configurada por materia/plan).
- Para **rendir examen**: requiere materias **aprobadas** (más estricto que cursar).
- Diferenciar estados:
  - `PENDING` (pendiente)
  - `ENROLLED` (inscripto)
  - `REGULAR` (regular)
  - `APPROVED` (aprobada)
  - `FAILED` (reprobada)

### Inscripción
- No permitir duplicados (ya inscripto).
- Cupos en mesas de examen.
- Choque de horario (si dos mesas se superponen).
- Ventana de inscripción (opcional v1.1; en v1 puede ser fija).

---

## 6) Datos ficticios

- 20 estudiantes
- 2 carreras / planes
- 35–45 materias
- 60–90 correlatividades
- 12–20 mesas de examen
- Historial de resultados y cursadas (para que “Estado académico” tenga contenido)

---

## 7) Modelo de datos mínimo (PostgreSQL)
Tablas sugeridas (MVP):
- `students` (id, legajo, full_name, email, password_hash, plan_id, created_at)
- `plans` (id, name, career, year_version)
- `subjects` (id, code, name, year, term)
- `plan_subjects` (plan_id, subject_id)
- `prerequisites` (subject_id, requires_subject_id, requirement_type)  
  `requirement_type`: `FOR_COURSE` | `FOR_EXAM` y/o `MIN_STATUS` (REGULAR/APPROVED)
- `course_enrollments` (id, student_id, subject_id, period, status)
- `exam_calls` (id, subject_id, starts_at, ends_at, capacity)
- `exam_enrollments` (id, student_id, exam_call_id, status, enrolled_at)
- `exam_results` (id, student_id, subject_id, date, grade, result_status)

---

## 8) Endpoints API (contrato inicial)
Auth:
- `POST /auth/login` -> JWT simple
- `GET /me`

Plan / estado:
- `GET /students/me/plan`
- `GET /students/me/status`

Exámenes:
- `GET /exam-calls?from=&to=&subjectId=`
- `POST /students/me/exam-enrollments` (inscribirse a mesa)
- `GET /students/me/exam-enrollments`

Cursado:
- `GET /course-offers?period=`
- `POST /students/me/course-enrollments` (inscribirse a cursado)
- `GET /students/me/course-enrollments`

Errores (estándar):
- 401/403 auth
- 409 conflicto (ya inscripto, cupo lleno)
- 422 validación (correlativas, choque horario)

---

## 9) Estructura de repo
/apps/web-angular
/api/Autogestion.Api
/api/Autogestion.Application
/api/Autogestion.Domain
/api/Autogestion.Infrastructure
/docs
/adr
erd.md
api-contract.md
README.md
README.es.md
docker-compose.yml


---

## 10) Git workflow
### Branches
- `main`: estable, listo para mostrar
- `develop`: integración diaria
- `feature/<scope>-<breve>`: trabajo corto (1–3 días máx)

### Regla de oro de commits
- 1 a 3 commits por día, pequeños (10–60 min c/u)
- Mensajes con convención:
  - `feat(api): ...`
  - `feat(web): ...`
  - `test(domain): ...`
  - `docs: ...`
  - `chore: ...`

### Seguimiento diario y push mensual
- Diario: commits locales en `feature/*` (minimo 1 por dia).
- Diario: registro breve en `docs/commit-log.md` (fecha + resumen + hash).
- Mensual: merge/push de `feature/*` -> `develop` (PR o merge directo).
- Mensual: merge/push de `develop` -> `main` y tag `v0.x.0`.

---

## 11) Definition of Done (MVP)
- Levanta local: Postgres (Docker) + API + Web
- Swagger funcionando
- Seed aplicado (migraciones + datos)
- Flujo completo:
  - login -> menú -> plan -> estado -> exam calls -> inscribirse -> ver inscripciones
  - ofertas de cursado -> inscribirse -> ver cursadas
- Validaciones clave (correlatividades + duplicados + cupo + choque horario)
- Tests mínimos:
  - unit tests de reglas de correlatividad
  - integration test básico de inscripción (opcional si el tiempo aprieta)
- README (EN + ES) con pasos para correr y capturas

---

## 12) Workflow de lo que falta para que la web sea funcional
Objetivo: front-end navegable, todos los botones operativos, API conectada, y datos persistidos en Postgres.

### Fase A: Inventario y decisiones (1-2 dias)
- Revisar paginas/flows existentes en Angular y mapear botones sin accion.
- Listar endpoints reales vs. los que faltan en API.
- Definir endpoints minimos para cada boton (DTOs y respuestas).

### Fase B: Backend funcional (API + DB)
1) Base de datos conectada y migraciones aplicables.
2) Seed minimo (planes, materias, alumnos, correlativas, exam calls).
3) Endpoints obligatorios (lectura + inscripcion):
   - `POST /auth/login`, `GET /me`
   - `GET /students/me/plan`
   - `GET /students/me/status`
   - `GET /exam-calls`
   - `POST /students/me/exam-enrollments`
   - `GET /students/me/exam-enrollments`
   - `GET /course-offers`
   - `POST /students/me/course-enrollments`
   - `GET /students/me/course-enrollments`
4) Validaciones clave: correlatividades, duplicados, cupo, choque horario.

### Fase C: Front-end funcional (Angular)
1) Autenticacion + guard + interceptor.
2) Menus y rutas estables.
3) Pantallas con datos reales (plan, estado, examenes, cursado).
4) Botones conectados a servicios (inscribirse, ver inscripciones).
5) Estados UX basicos: loading, empty, error.

### Fase D: Integracion y cierre
1) Flujo end-to-end:
   login -> menu -> plan -> estado -> examenes -> inscripcion -> mis examenes -> cursado -> inscripcion -> mis cursadas.
2) Tests minimos de reglas de dominio.
3) README actualizado con pasos para correr y capturas.

## 13) Plantilla de Issues (para historial pro)
Formato sugerido por día:
- Título: `Day X — <feature>`
- Checklist:
  - [ ] Implementación
  - [ ] Tests (si aplica)
  - [ ] Docs/README breve
  - [ ] PR a develop

---

## 14) Criterios de “commit liviano” (para mantener constancia)
Un commit liviano puede ser:
- una página Angular sin datos reales (solo layout)
- un endpoint GET sin reglas complejas
- una migración + seed
- 2–3 tests unitarios
- doc: ERD/ADR/README update

---

## 15) Próximo paso (antes de programar)
Crear en GitHub:
- Repo público + branches `main` y `develop`
- Branch protection (opcional pero recomendado):
  - PR required a `main`
  - CI required
- Issues por Semana 1 (Día 1–4)

Listo para arrancar con “Día 1”.
