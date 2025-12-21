# RefactoredSysacadUTNDELTA

Demo modernization of an academic self-service system.

## API (Autogestion.Api)
Purpose: minimal Web API host for the demo backend.

Usage:
- Run `dotnet run --project api/Autogestion.Api`
- Swagger is registered via Swashbuckle (https://aka.ms/aspnetcore/swashbuckle) and is enabled in Development at `/swagger`.
- HTTPS redirection is enabled by default.
- Endpoints are not implemented yet; they will cover students, subjects, enrollments, and exams.
