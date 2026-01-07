using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Exams;

public interface IEnrollExamCommand
{
    Task<Result> ExecuteAsync(int studentId, int examCallId, CancellationToken ct);
}