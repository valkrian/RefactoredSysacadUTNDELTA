using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Domain.Entities;
using Autogestion.Domain.Enums;
using Autogestion.Domain.Services;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Exams;

public sealed class EnrollExamCommand : IEnrollExamCommand
{
    private readonly ApplicationDbContext _dbContext;

    public EnrollExamCommand(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> ExecuteAsync(int studentId, int examCallId, CancellationToken ct)
    {
        var examCall = await _dbContext.ExamCalls
            .Include(e => e.Enrollments)
            .FirstOrDefaultAsync(e => e.Id == examCallId, ct);

        if (examCall is null)
        {
            return Result.Fail("Exam call not found.");
        }

        var alreadyEnrolled = await _dbContext.ExamEnrollments
            .AnyAsync(e => e.StudentId == studentId && e.ExamCallId == examCallId, ct);

        if (alreadyEnrolled)
        {
            return Result.Fail("Already enrolled in exam call.");
        }

        if (examCall.Enrollments.Count >= examCall.Capacity)
        {
            return Result.Fail("Exam call is full.");
        }

        var overlaps = await _dbContext.ExamEnrollments
            .Include(e => e.ExamCall)
            .AnyAsync(e =>
                e.StudentId == studentId &&
                e.ExamCall.StartsAt < examCall.EndsAt &&
                e.ExamCall.EndsAt > examCall.StartsAt, ct);

        if (overlaps)
        {
            return Result.Fail("Exam call overlaps with another enrollment.");
        }

        var prerequisites = await _dbContext.Prerequisites
            .AsNoTracking()
            .ToListAsync(ct);

        var courseEnrollments = await _dbContext.CourseEnrollments
            .Where(c => c.StudentId == studentId)
            .AsNoTracking()
            .ToListAsync(ct);

        var examResults = await _dbContext.ExamResults
            .Where(r => r.StudentId == studentId)
            .AsNoTracking()
            .ToListAsync(ct);

        if (!CorrelativityRules.CanEnrollExam(examCall.SubjectId, prerequisites, courseEnrollments, examResults))
        {
            return Result.Fail("Prerequisites not met for exam enrollment.");
        }

        var enrollment = new ExamEnrollment
        {
            StudentId = studentId,
            ExamCallId = examCall.Id,
            Status = ExamEnrollmentStatus.Enrolled,
            EnrolledAt = DateTime.UtcNow
        };

        _dbContext.ExamEnrollments.Add(enrollment);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
