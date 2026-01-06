using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Status;

public interface IGetStudentStatusQuery
{
    Task<Result<StudentStatusDto>> ExecuteAsync(int studentId, CancellationToken ct);
}