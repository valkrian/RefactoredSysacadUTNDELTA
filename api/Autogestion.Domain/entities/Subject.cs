namespace Autogestion.Domain.Entities;

public class Subject
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Term { get; set; }

    public ICollection<Plan> Plans { get; set; } = new List<Plan>();

}
