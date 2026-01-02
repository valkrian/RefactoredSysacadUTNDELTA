using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Plans;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Plans;

public sealed class GetStudentPlanQuery : IGetStudentPlanQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetStudentPlanQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PlanDto>> ExecuteAsync(int studentId, CancellationToken ct)
    {
        var student = await _dbContext.Students
            .Include(s => s.Plan)
            .ThenInclude(p => p.Subjects)
            .FirstOrDefaultAsync(s => s.Id == studentId, ct);

        if (student?.Plan is null)
        {
            return Result<PlanDto>.Fail("Plan not found.");
        }

        var plan = student.Plan;

        var dto = new PlanDto
        {
            PlanId = plan.Id,
            PlanName = plan.Name,
            Career = plan.Career,
            YearVersion = plan.YearVersion,
            Subjects = plan.Subjects
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Term)
                .ThenBy(s => s.Name)
                .Select(s => new PlanSubjectDto
                {
                    SubjectId = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    Year = s.Year,
                    Term = s.Term
                })
                .ToList()
        };

        return Result<PlanDto>.Ok(dto);
    }
}
