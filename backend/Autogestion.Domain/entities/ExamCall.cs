namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents an exam call with capacity and schedule window.
/// </summary>
public class ExamCall
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public int Capacity { get; set; }

    public Subject Subject { get; set; } = null!;
    public ICollection<ExamEnrollment> Enrollments { get; set; } = new List<ExamEnrollment>();
}
