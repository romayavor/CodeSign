using CodeSign.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeSign.API.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<SignatureTemplate> SignatureTemplates => Set<SignatureTemplate>();
    public DbSet<UserSignature> UserSignatures => Set<UserSignature>();
}