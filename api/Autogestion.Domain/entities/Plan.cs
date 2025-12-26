namespace Autogestion.Domain.Entities;

public class Plan 

{
    public int Id { get; set; }
    public string Name { get; set; }  = string.Empty;
    public string Career { get; set;} = string.Empty;
    public int YearVersion { get; set; }

//relationss
public ICollection<Student> Students { get; set; } = new List<Student>();

public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

}