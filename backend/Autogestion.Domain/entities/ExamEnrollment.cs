using Autogestion.Domain.Enums;

namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents a student enrollment in a specific exam call.
/// </summary>
public class ExamEnrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ExamCallId { get; set; }
    public ExamEnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; }

    public Student Student { get; set; } = null!;
    public ExamCall ExamCall { get; set; } = null!;
}
