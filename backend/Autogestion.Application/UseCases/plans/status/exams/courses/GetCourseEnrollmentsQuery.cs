using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Courses;

public interface IGetCourseEnrollmentsQuery
{
    Task<Result<List<CourseEnrollmentDto>>> ExecuteAsync(int studentId, CancellationToken ct);
}
