using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Api.DTOs;
using ArchaeologicalTimeMachine.Api.Services;
using ArchaeologicalTimeMachine.Domain.Entities;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtefactsController : ControllerBase
{
    private readonly ArchaeologyDbContext _context;

    public ArtefactsController(ArchaeologyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtefactDto>>> GetArtefacts(
        [FromQuery] string? material = null,
        [FromQuery] string? artefactType = null,
        [FromQuery] int? siteId = null,
        [FromQuery] string? search = null)
    {
        IQueryable<Artefact> query = _context.Artefacts.Include(a => a.Site);

        if (siteId.HasValue)
        {
            query = query.Where(a => a.SiteId == siteId.Value);
        }

        if (!string.IsNullOrWhiteSpace(material))
        {
            string cleanMat = material.Trim().ToLower();
            query = query.Where(a => a.Material.ToLower().Contains(cleanMat));
        }

        if (!string.IsNullOrWhiteSpace(artefactType))
        {
            string cleanType = artefactType.Trim().ToLower();
            query = query.Where(a => a.ArtefactType.ToLower().Contains(cleanType));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            string cleanSearch = search.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(cleanSearch) ||
                                     a.Description.ToLower().Contains(cleanSearch) ||
                                     a.CurrentLocation.ToLower().Contains(cleanSearch));
        }

        var artefacts = await query
            .OrderBy(a => a.ApproximateYear)
            .ToListAsync();

        return Ok(artefacts.Select(a => a.ToDto()).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArtefactDto>> GetArtefactById(int id)
    {
        var artefact = await _context.Artefacts
            .Include(a => a.Site)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artefact == null)
        {
            return NotFound(new { message = $"Artefact with ID {id} was not found." });
        }

        return Ok(artefact.ToDto());
    }
}
