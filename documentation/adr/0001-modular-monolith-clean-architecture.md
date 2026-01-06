# ADR 0001: Modular Monolith and Clean Architecture

Status: Accepted
Date: 2025-12-21

## Context
We need a demo that is easy to run locally while keeping a clear separation for domain rules.

## Decision
Use a modular monolith with Clean Architecture layers:
- Autogestion.Domain
- Autogestion.Application
- Autogestion.Infrastructure
- Autogestion.Api

## Consequences
- Clear boundaries for domain rules and tests.
- More projects and wiring upfront.
- The structure can evolve to services if needed.
