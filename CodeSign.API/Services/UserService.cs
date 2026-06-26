using CodeSign.API.Data;
using CodeSign.API.DTOs;
using CodeSign.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeSign.API.Services;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserService(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<UserProfileDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserProfileDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userSignature = await _context.UserSignatures
                .Include(us => us.Template)
                .FirstOrDefaultAsync(us => us.UserId == user.Id);

            result.Add(new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                JobTitle = user.JobTitle,
                Role = roles.FirstOrDefault() ?? "User",
                AssignedSignature = userSignature == null ? null : new SignatureTemplateDto
                {
                    Id = userSignature.Template.Id,
                    Name = userSignature.Template.Name,
                    HtmlContent = userSignature.Template.HtmlContent,
                    AccentColor = userSignature.Template.AccentColor,
                    CreatedAt = userSignature.Template.CreatedAt
                }
            });
        }

        return result;
    }

    public async Task<UserProfileDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var userSignature = await _context.UserSignatures
            .Include(us => us.Template)
            .FirstOrDefaultAsync(us => us.UserId == userId);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            JobTitle = user.JobTitle,
            Role = roles.FirstOrDefault() ?? "User",
            AssignedSignature = userSignature == null ? null : new SignatureTemplateDto
            {
                Id = userSignature.Template.Id,
                Name = userSignature.Template.Name,
                HtmlContent = userSignature.Template.HtmlContent,
                AccentColor = userSignature.Template.AccentColor,
                CreatedAt = userSignature.Template.CreatedAt
            }
        };
    }

    public async Task<bool> AssignSignatureAsync(string userId, int templateId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var template = await _context.SignatureTemplates.FindAsync(templateId);
        if (template == null) return false;

        // Видаляємо попередній підпис якщо є
        var existing = await _context.UserSignatures
            .FirstOrDefaultAsync(us => us.UserId == userId);

        if (existing != null)
            _context.UserSignatures.Remove(existing);

        _context.UserSignatures.Add(new UserSignature
        {
            UserId = userId,
            TemplateId = templateId,
            AssignedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveSignatureAsync(string userId)
    {
        var existing = await _context.UserSignatures
            .FirstOrDefaultAsync(us => us.UserId == userId);

        if (existing == null) return false;

        _context.UserSignatures.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}