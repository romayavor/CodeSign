using CodeSign.API.Data;
using CodeSign.API.DTOs;
using CodeSign.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeSign.API.Services;

public class SignatureService
{
    private readonly AppDbContext _context;

    public SignatureService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SignatureTemplateDto>> GetAllAsync()
    {
        return await _context.SignatureTemplates
            .Select(t => new SignatureTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                HtmlContent = t.HtmlContent,
                AccentColor = t.AccentColor,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<SignatureTemplateDto?> GetByIdAsync(int id)
    {
        var template = await _context.SignatureTemplates.FindAsync(id);
        if (template == null) return null;

        return new SignatureTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            HtmlContent = template.HtmlContent,
            AccentColor = template.AccentColor,
            CreatedAt = template.CreatedAt
        };
    }

    public async Task<SignatureTemplateDto> CreateAsync(CreateSignatureTemplateDto dto)
    {
        var template = new SignatureTemplate
        {
            Name = dto.Name,
            HtmlContent = dto.HtmlContent,
            AccentColor = dto.AccentColor,
            CreatedAt = DateTime.UtcNow
        };

        _context.SignatureTemplates.Add(template);
        await _context.SaveChangesAsync();

        return new SignatureTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            HtmlContent = template.HtmlContent,
            AccentColor = template.AccentColor,
            CreatedAt = template.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, CreateSignatureTemplateDto dto)
    {
        var template = await _context.SignatureTemplates.FindAsync(id);
        if (template == null) return false;

        template.Name = dto.Name;
        template.HtmlContent = dto.HtmlContent;
        template.AccentColor = dto.AccentColor;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var template = await _context.SignatureTemplates.FindAsync(id);
        if (template == null) return false;

        _context.SignatureTemplates.Remove(template);
        await _context.SaveChangesAsync();
        return true;
    }
}