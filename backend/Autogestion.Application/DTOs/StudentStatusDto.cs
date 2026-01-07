namespace Autogestion.Application.DTOs;

public sealed class StudentStatusDto
{
    public int StudentId { get; set; }
    public int ApprovedCount { get; set; }
    public int RegularCount { get; set; }
    public int PendingCount { get; set; }
    public decimal? Average { get; set; }
}
