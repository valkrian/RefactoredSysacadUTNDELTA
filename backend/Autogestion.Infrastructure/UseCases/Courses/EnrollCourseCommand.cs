using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Domain.Entities;
using Autogestion.Domain.Enums;
using Autogestion.Domain.Services;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Courses;

public sealed class EnrollCourseCommand : IEnrollCourseCommand
{
    private readonly ApplicationDbContext _dbContext;

    public EnrollCourseCommand(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> ExecuteAsync(int studentId, int subjectId, string period, CancellationToken ct)
    {
        var student = await _dbContext.Students
            .Include(s => s.Plan)
            .ThenInclude(p => p.Subjects)
            .FirstOrDefaultAsync(s => s.Id == studentId, ct);

        if (student?.Plan is null)
        {
            return Result.Fail("Student plan not found.");
        }

        var subjectInPlan = student.Plan.Subjects.Any(s => s.Id == subjectId);
        if (!subjectInPlan)
        {
            return Result.Fail("Subject not part of the student plan.");
        }

        var alreadyEnrolled = await _dbContext.CourseEnrollments
            .AnyAsync(e => e.StudentId == studentId && e.SubjectId == subjectId, ct);

        if (alreadyEnrolled)
        {
            return Result.Fail("Already enrolled in course.");
        }

        var approved = await _dbContext.ExamResults
            .AnyAsync(r => r.StudentId == studentId && r.SubjectId == subjectId && r.Status == ExamResultStatus.Approved, ct);

        if (approved)
        {
            return Result.Fail("Subject already approved.");
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

        if (!CorrelativityRules.CanEnrollCourse(subjectId, prerequisites, courseEnrollments, examResults))
        {
            return Result.Fail("Prerequisites not met for course enrollment.");
        }

        var enrollment = new CourseEnrollment
        {
            StudentId = studentId,
            SubjectId = subjectId,
            Period = period,
            Status = CourseEnrollmentStatus.Enrolled
        };

        _dbContext.CourseEnrollments.Add(enrollment);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
