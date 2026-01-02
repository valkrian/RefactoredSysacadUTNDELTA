using Autogestion.Domain.Enums;

namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents the final outcome of a student exam attempt.
/// </summary>
public class ExamResult
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public DateTime Date { get; set; }
    public int Grade { get; set; }
    public ExamResultStatus Status { get; set; }

    public Student Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
