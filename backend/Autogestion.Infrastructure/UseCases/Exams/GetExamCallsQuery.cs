using Autogestion.Application.DTOs;
using Autogestion.Application.Shared;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Infrastructure.Data;

namespace Autogestion.Infrastructure.UseCases.Exams;

public sealed class GetExamCallsQuery : IGetExamCallsQuery
{
    private readonly ApplicationDbContext _dbContext;

    public GetExamCallsQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Result<List<ExamCallDto>>> ExecuteAsync(DateTime? from, DateTime? to, int? subjectId, CancellationToken ct)
    {
        var empty = new List<ExamCallDto>();
        return Task.FromResult(Result<List<ExamCallDto>>.Ok(empty));
    }
}
