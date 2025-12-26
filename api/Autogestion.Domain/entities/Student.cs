namespace Autogestion.Domain.Entities;

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

}