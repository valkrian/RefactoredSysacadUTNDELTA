using Autogestion.Application.DTOs;
using Autogestion.Application.Interfaces;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.Services;

public sealed class PlanService : IPlanService
{
    private readonly ApplicationDbContext _dbContext;

    public PlanService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlanDto?> GetPlanForStudentAsync(int studentId, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.Plan)
            .ThenInclude(p => p.Subjects)
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student?.Plan is null)
        {
            return null;
        }

        return new PlanDto
        {
            PlanId = student.Plan.Id,
            PlanName = student.Plan.Name,
            Career = student.Plan.Career,
            YearVersion = student.Plan.YearVersion,
            Subjects = student.Plan.Subjects
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
    }
}
