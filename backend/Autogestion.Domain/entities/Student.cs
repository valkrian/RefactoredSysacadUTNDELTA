namespace Autogestion.Domain.Entities;

/// <summary>
/// Represents a student and their academic relationships.
/// </summary>
public class Student
{
public int Id { get; set; } 
public string Legajo { get; set; } = string.Empty;
public string FullName { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string PasswordHash { get; set; } = string.Empty;
public int PlanId { get; set; }
public Plan Plan { get; set; } = null!;
public DateTime CreatedAt { get; set; }

public ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();
public ICollection<ExamEnrollment> ExamEnrollments { get; set; } = new List<ExamEnrollment>();
public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

}
