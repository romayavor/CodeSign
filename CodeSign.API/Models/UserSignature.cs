namespace CodeSign.API.Models;

public class UserSignature
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TemplateId { get; set; }
    public SignatureTemplate Template { get; set; } = null!;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}