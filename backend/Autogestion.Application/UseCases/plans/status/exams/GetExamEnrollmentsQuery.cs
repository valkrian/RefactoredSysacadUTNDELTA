using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Exams;

public interface IGetExamEnrollmentsQuery
{
    Task<Result<List<ExamEnrollmentDto>>> ExecuteAsync(int studentId, CancellationToken ct);
}
