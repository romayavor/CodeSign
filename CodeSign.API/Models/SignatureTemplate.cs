namespace CodeSign.API.Models;

public class SignatureTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string AccentColor { get; set; } = "#185FA5";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}