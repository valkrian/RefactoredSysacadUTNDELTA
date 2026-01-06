using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;

namespace Autogestion.Application.UseCases.Plans;

public interface IGetStudentPlanQuery
{
    Task<Result<PlanDto>> ExecuteAsync(int studentId, CancellationToken ct);
}