namespace Autogestion.Application.DTOs;

public sealed class PlanDto
{
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string Career { get; set; } = string.Empty;
    public int YearVersion { get; set; }
    public List<PlanSubjectDto> Subjects { get; set; } = new();
}

public sealed class PlanSubjectDto
{
    public int SubjectId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Term { get; set; }
}

