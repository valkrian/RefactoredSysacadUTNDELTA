using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Courses;

public sealed class GetCourseEnrollmentsQuery : IGetCourseEnrollmentsQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetCourseEnrollmentsQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<CourseEnrollmentDto>>> ExecuteAsync(int studentId, CancellationToken ct)
    {
        var enrollments = await _dbContext.CourseEnrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Subject)
            .AsNoTracking()
            .OrderByDescending(e => e.Period)
            .ThenBy(e => e.Subject.Name)
            .Select(e => new CourseEnrollmentDto
            {
                SubjectId = e.SubjectId,
                SubjectName = e.Subject.Name,
                Period = e.Period,
                Status = e.Status.ToString()
            })
            .ToListAsync(ct);

        return Result<List<CourseEnrollmentDto>>.Ok(enrollments);
    }
}
