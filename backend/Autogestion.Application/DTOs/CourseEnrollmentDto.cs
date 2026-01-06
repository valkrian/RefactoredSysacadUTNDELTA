namespace Autogestion.Application.DTOs;

public sealed class CourseEnrollmentDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
