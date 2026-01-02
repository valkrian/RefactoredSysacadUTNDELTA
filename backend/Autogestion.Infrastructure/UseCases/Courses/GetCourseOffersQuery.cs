using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Infrastructure.Data;

namespace Autogestion.Infrastructure.UseCases.Courses;

public sealed class GetCourseOffersQuery : IGetCourseOffersQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetCourseOffersQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Result<List<CourseOfferDto>>> ExecuteAsync(string? period, CancellationToken ct)
    {
        var empty = new List<CourseOfferDto>();
        return Task.FromResult(Result<List<CourseOfferDto>>.Ok(empty));
    }
}
