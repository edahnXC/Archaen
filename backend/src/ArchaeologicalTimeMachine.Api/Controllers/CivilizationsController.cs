using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Api.DTOs;
using ArchaeologicalTimeMachine.Api.Services;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CivilizationsController : ControllerBase
{
    private readonly ArchaeologyDbContext _context;

    public CivilizationsController(ArchaeologyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CivilizationDto>>> GetCivilizations()
    {
        var civilizations = await _context.Civilizations
            .Include(c => c.SiteCivilizations)
            .OrderBy(c => c.StartYear)
            .ToListAsync();

        var dtos = civilizations.Select(c => c.ToDto(c.SiteCivilizations.Count)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CivilizationDto>> GetCivilizationById(int id)
    {
        var civilization = await _context.Civilizations
            .Include(c => c.SiteCivilizations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (civilization == null)
        {
            return NotFound(new { message = $"Civilization {id} not found." });
        }

        return Ok(civilization.ToDto(civilization.SiteCivilizations.Count));
    }

    [HttpGet("{id:int}/sites")]
    public async Task<ActionResult<List<SiteDto>>> GetSitesByCivilization(int id)
    {
        var sites = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Where(s => s.SiteCivilizations.Any(sc => sc.CivilizationId == id))
            .OrderBy(s => s.StartYear)
            .ToListAsync();

        return Ok(sites.Select(s => s.ToDto()).ToList());
    }
}
