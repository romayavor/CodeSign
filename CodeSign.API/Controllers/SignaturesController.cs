using CodeSign.API.DTOs;
using CodeSign.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SignaturesController : ControllerBase
{
    private readonly SignatureService _service;

    public SignaturesController(SignatureService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<SignatureTemplateDto>>> GetAll()
    {
        var templates = await _service.GetAllAsync();
        return Ok(templates);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SignatureTemplateDto>> GetById(int id)
    {
        var template = await _service.GetByIdAsync(id);
        if (template == null) return NotFound();
        return Ok(template);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SignatureTemplateDto>> Create(CreateSignatureTemplateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, CreateSignatureTemplateDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}