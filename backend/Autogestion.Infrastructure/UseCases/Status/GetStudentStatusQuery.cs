using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Status;
using Autogestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autogestion.Infrastructure.UseCases.Status;

public sealed class GetStudentStatusQuery : IGetStudentStatusQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetStudentStatusQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<StudentStatusDto>> ExecuteAsync(int studentId, CancellationToken ct)
    {
        var student = await _dbContext.Students
            .Include(s => s.Plan)
            .ThenInclude(p => p.Subjects)
            .FirstOrDefaultAsync(s => s.Id == studentId, ct);

        if (student?.Plan is null)
        {
            return Result<StudentStatusDto>.Fail("Plan not found.");
        }

        var subjectCount = student.Plan.Subjects.Count;

        var dto = new StudentStatusDto
        {
            StudentId = student.Id,
            ApprovedCount = 0,
            RegularCount = 0,
            PendingCount = subjectCount,
            Average = null
        };

        return Result<StudentStatusDto>.Ok(dto);
    }
}
