using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Courses;

public interface IEnrollCourseCommand
{
    Task<Result> ExecuteAsync(int studentId, int subjectId, string period, CancellationToken ct);
}
