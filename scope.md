# scope.md — Modernización Autogestión Alumnos (Demo Portfolio)

## 1) Objetivo
Modernizar la experiencia de “Autogestión Alumnos” (legacy ASP) en un **demo funcional** con:
- UI moderna tipo sistema administrativo (Bootstrap)
- Backend realista con reglas de negocio (correlatividades intermedias)
- Base de datos **ficticia** en **PostgreSQL**
- Historial de trabajo visible: **commits diarios** + **release semanal** usando `develop/main`

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
### Enfoque recomendado: Monolito modular por capas (Clean Architecture liviana)
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

## 5) Reglas de negocio (nivel intermedio)
### Correlatividades (intermedio)
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

## 6) Datos ficticios (seed)
### Cantidad sugerida para demo
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

## 10) Git workflow (realista)
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

### Pull Requests
- Diario: `feature/*` -> PR a `develop`
- Semanal: PR `develop` -> `main` + tag `v0.x.0`

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

## 12) Cronograma de trabajo (commits diarios + resumen semanal)
> Diseño para baja carga: cada día = 1 “bloque” principal + 1 commit corto de docs/tests si da el tiempo.

### Semana 1 — Setup + bases (4 días)
**Objetivo semanal:** repo listo, Postgres docker, esqueletos API/Web, CI básico, docs iniciales.

**Día 1**
- Crear repo + estructura carpetas.
- Crear `README.md` y `README.es.md` (placeholder) + `scope.md`.
- Commit(s):
  - `docs: add scope and readmes`
  - `chore: init repo structure`

**Día 2**
- Crear solución .NET + proyectos por capas (vacíos).
- Swagger + health endpoint.
- Commit(s):
  - `feat(api): bootstrap web api + swagger`
  - `chore: add solution structure`

**Día 3**
- Angular app base + Bootstrap integrado + routing base.
- Layout “admin” (navbar/sidebar simple).
- Commit(s):
  - `feat(web): init angular app + bootstrap`
  - `feat(web): add base layout + routing`

**Día 4**
- Docker compose solo Postgres + variables de entorno.
- GitHub Actions build (API + Web).
- Commit(s):
  - `chore: add docker postgres`
  - `chore(ci): add build pipeline`

**Total Semana 1 (resultado visible)**
- Repo público “arranca” en cualquier PC.
- Web muestra layout.
- API responde health y Swagger.
- Postgres docker listo.

---

### Semana 2 — DB + seeds + contrato API (7 días, pero liviano)
**Objetivo semanal:** esquema de DB + migraciones + seed + endpoints de lectura para plan/estado.

**Día 1**
- ERD en `/docs/erd.md` + decisiones (mini ADR).
- Commit: `docs: add ERD + initial ADRs`

**Día 2**
- EF Core + conexión Postgres + migrations iniciales (plans/students/subjects).
- Commit: `feat(api): add efcore + initial migrations`

**Día 3**
- Tablas correlatividades + enrollments + exam_calls.
- Commit: `feat(api): add academic tables (prereqs/enrollments/exams)`

**Día 4**
- Seed data (planes, materias, alumnos).
- Commit: `feat(api): add seed data v1`

**Día 5**
- Endpoint `GET /students/me/plan` + DTOs.
- Commit: `feat(api): add plan endpoint`

**Día 6**
- Endpoint `GET /students/me/status` + cálculo simple (aprobadas/regularizadas).
- Commit: `feat(api): add academic status endpoint`

**Día 7 (commit corto)**
- Swagger examples + doc `api-contract.md`.
- Commit: `docs: add api contract + swagger examples`

**Total Semana 2**
- DB real con datos fake.
- Plan y Estado académico ya se ven desde API.

---

### Semana 3 — Auth + UI conectada (7 días)
**Objetivo semanal:** JWT simple + login UI + menú funcional + pantallas Plan/Estado consumiendo API.

**Día 1**
- `POST /auth/login` + `GET /me` (JWT).
- Commit: `feat(api): add jwt auth (simple)`

**Día 2**
- Angular login + guards + interceptor token.
- Commit: `feat(web): add login + auth flow`

**Día 3**
- Dashboard / Menú (cards links) + consumo de `/me`.
- Commit: `feat(web): add dashboard menu + me`

**Día 4**
- Pantalla Materias del plan (tabla + filtros).
- Commit: `feat(web): add plan page`

**Día 5**
- Pantalla Estado académico (tabs: aprobadas/regularizadas/pendientes).
- Commit: `feat(web): add academic status page`

**Día 6 (commit corto)**
- UX: loading/error, mensajes.
- Commit: `feat(web): improve UX states`

**Día 7 (commit corto)**
- Docs + screenshots iniciales.
- Commit: `docs: add screenshots + run guide`

**Total Semana 3**
- Login funcional.
- Plan y Estado académico visibles en UI.

---

### Semana 4 — Exámenes + Inscripción (7 días)
**Objetivo semanal:** mesas de examen + inscripción con reglas (cupo/choque/correlativas rendir).

**Día 1**
- `GET /exam-calls` + filtros.
- Commit: `feat(api): list exam calls`

**Día 2**
- Regla correlatividad para rendir (Domain service) + validaciones.
- Commit: `feat(domain): exam prerequisites rule`

**Día 3**
- `POST /students/me/exam-enrollments` con duplicados/cupo/choque.
- Commit: `feat(api): exam enrollment with validations`

**Día 4**
- UI Exámenes: listado mesas + botón inscribirse.
- Commit: `feat(web): exam calls page + enrollment`

**Día 5**
- UI “Mis inscripciones” (exámenes).
- Commit: `feat(web): my exam enrollments page`

**Día 6 (commit corto)**
- Tests unitarios de reglas (correlativas/cupo/choque).
- Commit: `test(domain): add exam enrollment rules tests`

**Día 7 (commit corto)**
- Release semanal: PR develop->main + tag `v0.4.0` + changelog.
- Commit: `chore: release v0.4.0`

**Total Semana 4**
- Exámenes e inscripción demostrables end-to-end.

---

### Semana 5 — Cursado + Inscripción (7 días)
**Objetivo semanal:** ofertas de cursado + inscripción con reglas (correlativas cursar).

**Día 1**
- `GET /course-offers?period=` (puede ser “mock-table” en DB).
- Commit: `feat(api): list course offers`

**Día 2**
- Regla correlatividad para cursar (Domain).
- Commit: `feat(domain): course prerequisites rule`

**Día 3**
- `POST /students/me/course-enrollments` con validaciones.
- Commit: `feat(api): course enrollment with validations`

**Día 4**
- UI Cursado: listado + inscripción.
- Commit: `feat(web): course offers page + enrollment`

**Día 5**
- UI “Mis cursadas”.
- Commit: `feat(web): my course enrollments page`

**Día 6 (commit corto)**
- Tests reglas cursado.
- Commit: `test(domain): add course enrollment rules tests`

**Día 7 (commit corto)**
- Release semanal: tag `v0.5.0` + README update.
- Commit: `docs: update readmes for v0.5.0`

**Total Semana 5**
- Cursado e inscripción demostrables end-to-end.

---

### Semana 6 — Pulido recruiter-ready (7 días, liviano)
**Objetivo semanal:** documentación, capturas, estabilidad, presentación, release `v1.0.0`.

**Día 1**
- README EN/ES final (cómo correr, arquitectura, endpoints).
- Commit: `docs: finalize readmes`

**Día 2**
- Documentación técnica: ADRs + decisiones (por qué Clean, por qué Postgres).
- Commit: `docs: add ADRs and decisions`

**Día 3**
- Hardening demo: validaciones input + CORS + rate limit básico (opcional).
- Commit: `chore(api): basic hardening`

**Día 4**
- UX polish (Bootstrap tables, forms, alerts consistentes).
- Commit: `refactor(web): polish UI consistency`

**Día 5**
- CI: tests + build + lint.
- Commit: `chore(ci): add tests and lint steps`

**Día 6 (commit corto)**
- Capturas + gif / video link.
- Commit: `docs: add demo media`

**Día 7 (commit corto)**
- Release final: PR develop->main, tag `v1.0.0`.
- Commit: `chore: release v1.0.0`

---

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
