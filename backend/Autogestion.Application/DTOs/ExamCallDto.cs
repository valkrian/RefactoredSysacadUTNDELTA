namespace Autogestion.Application.DTOs;
public sealed class ExamCallDto
{
    public int ExamCallId { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
}