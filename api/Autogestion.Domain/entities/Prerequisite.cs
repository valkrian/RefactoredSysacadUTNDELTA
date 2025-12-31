using Autogestion.Domain.Enums;

namespace Autogestion.Domain.Entities;

/// <summary>
/// Links a subject to a required subject and the minimum status needed.
/// </summary>
public class Prerequisite
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public int RequiresSubjectId { get; set; }
    public PrerequisiteType Type { get; set; }
    public MinimumSubjectStatus MinimumStatus { get; set; }

    public Subject Subject { get; set; } = null!;
    public Subject RequiresSubject { get; set; } = null!;
}
