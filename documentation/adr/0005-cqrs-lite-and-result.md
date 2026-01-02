# ADR 0005: CQRS liviano + Result pattern

## Contexto
Necesitamos separar lecturas y escrituras sin introducir complejidad excesiva.
Tambien queremos evitar excepciones para validar reglas de negocio y retornar
errores de forma consistente.

## Decision
- Implementar CQRS liviano con interfaces de casos de uso en Application.
- Implementar los casos de uso en Infrastructure usando EF Core.
- Usar un Result simple (`Result` y `Result<T>`) para comunicar exito o error.

## Consecuencias
- Lecturas y comandos quedan desacoplados por interfaces.
- Se evita el uso de excepciones para validaciones normales.
- Los endpoints pueden mapear Result a codigos HTTP de forma clara.
