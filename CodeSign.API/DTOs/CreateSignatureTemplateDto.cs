namespace CodeSign.API.DTOs;

public class CreateSignatureTemplateDto
{
    public string Name { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string AccentColor { get; set; } = "#185FA5";
}