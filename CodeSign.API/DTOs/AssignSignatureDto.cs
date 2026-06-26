namespace CodeSign.API.DTOs;

public class AssignSignatureDto
{
    public string UserId { get; set; } = string.Empty;
    public int TemplateId { get; set; }
}