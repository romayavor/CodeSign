using Microsoft.AspNetCore.Identity;

namespace CodeSign.API.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
}