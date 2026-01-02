using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Exams;

public interface IGetExamCallsQuery
{
    Task<Result<List<ExamCallDto>>> ExecuteAsync(DateTime? from, DateTime? to, int? subjectId, CancellationToken ct);
}