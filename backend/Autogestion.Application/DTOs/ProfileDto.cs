namespace Autogestion.Application.DTOs;
public sealed class ProfileDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Legajo { get; set; } = string.Empty;
}