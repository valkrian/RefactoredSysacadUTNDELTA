# ADR 0002: PostgreSQL via Docker Compose

Status: Accepted
Date: 2025-12-21

## Context
We need a realistic relational database with constraints and a setup that runs on any dev machine.

## Decision
Use PostgreSQL for the database, managed by docker-compose for local development. Use EF Core as the ORM.

## Consequences
- Real SQL semantics and constraints.
- Requires Docker installed for local runs.
- Connection strings and env vars must be documented.
