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
public class PeriodsController : ControllerBase
{
    private readonly ArchaeologyDbContext _context;

    public PeriodsController(ArchaeologyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<HistoricalPeriodDto>>> GetPeriods()
    {
        var periods = await _context.HistoricalPeriods
            .Include(p => p.SitePeriods)
            .OrderBy(p => p.StartYear)
            .ToListAsync();

        var dtos = periods.Select(p => p.ToDto(p.SitePeriods.Count)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HistoricalPeriodDto>> GetPeriodById(int id)
    {
        var period = await _context.HistoricalPeriods
            .Include(p => p.SitePeriods)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (period == null)
        {
            return NotFound(new { message = $"Historical period {id} not found." });
        }

        return Ok(period.ToDto(period.SitePeriods.Count));
    }

    [HttpGet("{id:int}/sites")]
    public async Task<ActionResult<List<SiteDto>>> GetSitesByPeriod(int id)
    {
        var sites = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Where(s => s.SitePeriods.Any(sp => sp.HistoricalPeriodId == id))
            .OrderBy(s => s.StartYear)
            .ToListAsync();

        return Ok(sites.Select(s => s.ToDto()).ToList());
    }
}
