using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Exams;

public sealed class GetExamEnrollmentsQuery : IGetExamEnrollmentsQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetExamEnrollmentsQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ExamEnrollmentDto>>> ExecuteAsync(int studentId, CancellationToken ct)
    {
        var enrollments = await _dbContext.ExamEnrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.ExamCall)
            .ThenInclude(ec => ec.Subject)
            .AsNoTracking()
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new ExamEnrollmentDto
            {
                ExamCallId = e.ExamCallId,
                SubjectId = e.ExamCall.SubjectId,
                SubjectName = e.ExamCall.Subject.Name,
                StartsAt = e.ExamCall.StartsAt,
                EndsAt = e.ExamCall.EndsAt,
                Status = e.Status.ToString(),
                EnrolledAt = e.EnrolledAt
            })
            .ToListAsync(ct);

        return Result<List<ExamEnrollmentDto>>.Ok(enrollments);
    }
}
