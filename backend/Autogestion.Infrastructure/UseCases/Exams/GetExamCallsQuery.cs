using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Exams;

public sealed class GetExamCallsQuery : IGetExamCallsQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetExamCallsQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ExamCallDto>>> ExecuteAsync(DateTime? from, DateTime? to, int? subjectId, CancellationToken ct)
    {
        var query = _dbContext.ExamCalls
            .Include(e => e.Subject)
            .Include(e => e.Enrollments)
            .AsNoTracking()
            .AsQueryable();

        if (from.HasValue)
        {
            query = query.Where(e => e.StartsAt >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(e => e.EndsAt <= to.Value);
        }

        if (subjectId.HasValue)
        {
            query = query.Where(e => e.SubjectId == subjectId.Value);
        }

        var calls = await query
            .OrderBy(e => e.StartsAt)
            .Select(e => new ExamCallDto
            {
                ExamCallId = e.Id,
                SubjectId = e.SubjectId,
                SubjectName = e.Subject.Name,
                StartsAt = e.StartsAt,
                EndsAt = e.EndsAt,
                Capacity = e.Capacity,
                EnrolledCount = e.Enrollments.Count
            })
            .ToListAsync(ct);

        return Result<List<ExamCallDto>>.Ok(calls);
    }
}
