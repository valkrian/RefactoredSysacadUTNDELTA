using Autogestion.Domain.Enums;

namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents a student enrollment in a course period.
/// </summary>
public class CourseEnrollment
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public string Period { get; set; } = string.Empty;
    public CourseEnrollmentStatus Status { get; set; }

    public Student Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
