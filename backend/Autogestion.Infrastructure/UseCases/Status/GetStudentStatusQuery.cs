using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Status;
using Autogestion.Domain.Enums;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Status;

public sealed class GetStudentStatusQuery : IGetStudentStatusQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetStudentStatusQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<StudentStatusDto>> ExecuteAsync(int studentId, CancellationToken ct)
    {
        var student = await _dbContext.Students
            .Include(s => s.Plan)
            .ThenInclude(p => p.Subjects)
            .FirstOrDefaultAsync(s => s.Id == studentId, ct);

        if (student?.Plan is null)
        {
            return Result<StudentStatusDto>.Fail("Plan not found.");
        }

        var subjectCount = student.Plan.Subjects.Count;

        var approvedSubjects = await _dbContext.ExamResults
            .Where(r => r.StudentId == studentId && r.Status == ExamResultStatus.Approved)
            .Select(r => r.SubjectId)
            .Distinct()
            .ToListAsync(ct);

        var regularSubjects = await _dbContext.CourseEnrollments
            .Where(c => c.StudentId == studentId && c.Status == CourseEnrollmentStatus.Regular)
            .Select(c => c.SubjectId)
            .Distinct()
            .ToListAsync(ct);

        var approvedCount = approvedSubjects.Count;
        var regularCount = regularSubjects.Except(approvedSubjects).Count();
        var pendingCount = Math.Max(0, subjectCount - approvedCount - regularCount);

        var dto = new StudentStatusDto
        {
            StudentId = student.Id,
            ApprovedCount = approvedCount,
            RegularCount = regularCount,
            PendingCount = pendingCount,
            Average = null
        };

        return Result<StudentStatusDto>.Ok(dto);
    }
}
