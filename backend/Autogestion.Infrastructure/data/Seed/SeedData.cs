using Autogestion.Domain.Entities;
using Autogestion.Domain.Enums;
using Autogestion.Infrastructure.Data;

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

        var subjectByCode = subjects.ToDictionary(s => s.Code);

        var prerequisites = new List<Prerequisite>
        {
            new()
            {
                Subject = subjectByCode["SO-201"],
                RequiresSubject = subjectByCode["PRG-103"],
                Type = PrerequisiteType.ForCourse,
                MinimumStatus = MinimumSubjectStatus.Regular
            },
            new()
            {
                Subject = subjectByCode["SO-201"],
                RequiresSubject = subjectByCode["PRG-103"],
                Type = PrerequisiteType.ForExam,
                MinimumStatus = MinimumSubjectStatus.Approved
            },
            new()
            {
                Subject = subjectByCode["BD-202"],
                RequiresSubject = subjectByCode["PRG-103"],
                Type = PrerequisiteType.ForCourse,
                MinimumStatus = MinimumSubjectStatus.Regular
            }
        };

        var courseEnrollments = new List<CourseEnrollment>
        {
            new()
            {
                StudentId = student.Id,
                SubjectId = subjectByCode["PRG-103"].Id,
                Period = "2024-1",
                Status = CourseEnrollmentStatus.Regular
            }
        };

        var examResults = new List<ExamResult>
        {
            new()
            {
                StudentId = student.Id,
                SubjectId = subjectByCode["PRG-103"].Id,
                Date = DateTime.UtcNow.AddMonths(-2),
                Grade = 8,
                Status = ExamResultStatus.Approved
            }
        };

        var examCalls = new List<ExamCall>
        {
            new()
            {
                SubjectId = subjectByCode["SO-201"].Id,
                StartsAt = DateTime.UtcNow.AddDays(7).Date.AddHours(9),
                EndsAt = DateTime.UtcNow.AddDays(7).Date.AddHours(11),
                Capacity = 30
            },
            new()
            {
                SubjectId = subjectByCode["BD-202"].Id,
                StartsAt = DateTime.UtcNow.AddDays(10).Date.AddHours(14),
                EndsAt = DateTime.UtcNow.AddDays(10).Date.AddHours(16),
                Capacity = 25
            }
        };

        dbContext.Prerequisites.AddRange(prerequisites);
        dbContext.CourseEnrollments.AddRange(courseEnrollments);
        dbContext.ExamResults.AddRange(examResults);
        dbContext.ExamCalls.AddRange(examCalls);
        dbContext.SaveChanges();
    }
}
