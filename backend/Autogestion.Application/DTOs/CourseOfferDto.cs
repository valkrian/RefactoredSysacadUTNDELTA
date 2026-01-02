namespace Autogestion.Application.DTOs;

public sealed class CourseOfferDto
{
    public int CourseOfferId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;

}
