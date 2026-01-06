using Autogestion.Application.DTOs;

namespace Autogestion.Application.Interfaces;

public interface IPlanService
{
    Task<PlanDto?> GetPlanForStudentAsync(int studentId, CancellationToken cancellationToken);
}
