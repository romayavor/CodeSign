using CodeSign.API.DTOs;
using CodeSign.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeSign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserProfileDto>>> GetAll()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var profile = await _service.GetUserByIdAsync(userId);
        if (profile == null) return NotFound();

        return Ok(profile);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserProfileDto>> GetById(string id)
    {
        var profile = await _service.GetUserByIdAsync(id);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    [HttpPost("{userId}/assign-signature")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignSignature(string userId, [FromBody] int templateId)
    {
        var success = await _service.AssignSignatureAsync(userId, templateId);
        if (!success) return BadRequest("Nie znaleziono użytkownika lub szablonu.");
        return Ok("Podpis został przypisany.");
    }

    [HttpDelete("{id}/signature")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveSignature(string id)
    {
        var success = await _service.RemoveSignatureAsync(id);
        if (!success) return NotFound("Nie znaleziono przypisanego podpisu.");
        return NoContent();
    }
}