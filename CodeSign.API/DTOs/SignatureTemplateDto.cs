namespace CodeSign.API.DTOs;

public class SignatureTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}