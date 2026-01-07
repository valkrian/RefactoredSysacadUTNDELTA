using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Courses;

public sealed class GetCourseOffersQuery : IGetCourseOffersQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetCourseOffersQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<CourseOfferDto>>> ExecuteAsync(string? period, CancellationToken ct)
    {
        var resolvedPeriod = string.IsNullOrWhiteSpace(period) ? "2024-1" : period;

        var offers = await _dbContext.Subjects
            .AsNoTracking()
            .OrderBy(s => s.Year)
            .ThenBy(s => s.Term)
            .ThenBy(s => s.Name)
            .Select(s => new CourseOfferDto
            {
                CourseOfferId = s.Id,
                SubjectId = s.Id,
                Code = s.Code,
                SubjectName = s.Name,
                Period = resolvedPeriod
            })
            .ToListAsync(ct);

        return Result<List<CourseOfferDto>>.Ok(offers);
    }
}
