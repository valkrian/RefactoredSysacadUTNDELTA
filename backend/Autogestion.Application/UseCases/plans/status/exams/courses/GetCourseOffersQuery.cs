using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
namespace Autogestion.Application.UseCases.Courses;

public interface IGetCourseOffersQuery
{
    Task<Result<List<CourseOfferDto>>> ExecuteAsync(string? period, CancellationToken ct);
}
