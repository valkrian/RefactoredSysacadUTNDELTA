using Autogestion.Domain.Entities;

namespace Autogestion.Infrastructure.Data.Seed;

public static class SeedData
{
    public static void EnsureSeedData(ApplicationDbContext dbContext)
    {
        if (dbContext.Plans.Any())
        {
            return;
        }

        var plan = new Plan
        {
            Name = "Ingenieria en Sistemas",
            Career = "Sistemas",
            YearVersion = 2024
        };

        var subjects = new List<Subject>
        {
            new() { Code = "MAT-101", Name = "Analisis Matematico I", Year = 1, Term = 1 },
            new() { Code = "ALG-102", Name = "Algebra Lineal", Year = 1, Term = 1 },
            new() { Code = "PRG-103", Name = "Programacion I", Year = 1, Term = 1 },
            new() { Code = "SO-201", Name = "Sistemas Operativos", Year = 2, Term = 1 },
            new() { Code = "BD-202", Name = "Bases de Datos", Year = 2, Term = 1 },
            new() { Code = "IS-203", Name = "Ingenieria de Software", Year = 2, Term = 2 }
        };

        foreach (var subject in subjects)
        {
            plan.Subjects.Add(subject);
        }

        var student = new Student
        {
            Legajo = "10001",
            FullName = "Alumno Demo",
            Email = "demo@utn.local",
            PasswordHash = "demo",
            Plan = plan,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Plans.Add(plan);
        dbContext.Students.Add(student);
        dbContext.SaveChanges();
    }
}
