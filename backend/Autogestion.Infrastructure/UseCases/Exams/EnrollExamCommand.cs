using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Infrastructure.Data;

namespace Autogestion.Infrastructure.UseCases.Exams;

public sealed class EnrollExamCommand : IEnrollExamCommand
{
    private readonly ApplicationDbContext _dbContext;

    public EnrollExamCommand(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Result> ExecuteAsync(int studentId, int examCallId, CancellationToken ct)
    {
        return Task.FromResult(Result.Fail("Exam enrollment not implemented."));
    }
}
