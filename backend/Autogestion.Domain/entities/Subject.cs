namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents a subject in a plan and its academic relationships.
/// </summary>
public class Subject
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Term { get; set; }

    public ICollection<Plan> Plans { get; set; } = new List<Plan>();
    public ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();
    public ICollection<ExamCall> ExamCalls { get; set; } = new List<ExamCall>();
    public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    public ICollection<Prerequisite> Prerequisites { get; set; } = new List<Prerequisite>();
    public ICollection<Prerequisite> RequiredBy { get; set; } = new List<Prerequisite>();

}
