using System.Linq;
using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Api.DTOs;
using ArchaeologicalTimeMachine.Api.Services;
using ArchaeologicalTimeMachine.Domain.Common;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/sites")]
public class SiteComparisonController : ControllerBase
{
    private readonly ArchaeologyDbContext _context;

    public SiteComparisonController(ArchaeologyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Compares two archaeological sites across spatial distance, chronological overlap, architectural attributes, civilizations, and artefacts.
    /// </summary>
    [HttpGet("compare")]
    public async Task<ActionResult<SiteComparisonDto>> CompareSites([FromQuery] int site1Id, [FromQuery] int site2Id)
    {
        if (site1Id == site2Id)
        {
            return BadRequest(new { message = "Cannot compare a site with itself. Please select two distinct archaeological sites." });
        }

        var s1 = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Include(s => s.Artefacts)
            .Include(s => s.Excavations).ThenInclude(e => e.Layers).ThenInclude(l => l.Findings)
            .Include(s => s.SiteReferences).ThenInclude(sr => sr.Reference)
            .Include(s => s.OutgoingRelationships).ThenInclude(r => r.TargetSite)
            .Include(s => s.IncomingRelationships).ThenInclude(r => r.SourceSite)
            .FirstOrDefaultAsync(s => s.Id == site1Id);

        var s2 = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Include(s => s.Artefacts)
            .Include(s => s.Excavations).ThenInclude(e => e.Layers).ThenInclude(l => l.Findings)
            .Include(s => s.SiteReferences).ThenInclude(sr => sr.Reference)
            .Include(s => s.OutgoingRelationships).ThenInclude(r => r.TargetSite)
            .Include(s => s.IncomingRelationships).ThenInclude(r => r.SourceSite)
            .FirstOrDefaultAsync(s => s.Id == site2Id);

        if (s1 == null || s2 == null)
        {
            return NotFound(new { message = "One or both archaeological sites were not found." });
        }

        double distance = SpatialHelper.CalculateDistanceKm(s1.Latitude, s1.Longitude, s2.Latitude, s2.Longitude);
        int overlapYears = TemporalHelper.OverlapDuration(s1.StartYear, s1.EndYear, s2.StartYear, s2.EndYear);
        bool isContemporaneous = TemporalHelper.Overlaps(s1.StartYear, s1.EndYear, s2.StartYear, s2.EndYear);

        var civs1 = s1.SiteCivilizations.Select(sc => sc.Civilization?.Name).Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
        var civs2 = s2.SiteCivilizations.Select(sc => sc.Civilization?.Name).Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
        var sharedCivs = civs1.Intersect(civs2).OfType<string>().ToList();

        var periods1 = s1.SitePeriods.Select(sp => sp.HistoricalPeriod?.Name).Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
        var periods2 = s2.SitePeriods.Select(sp => sp.HistoricalPeriod?.Name).Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
        var sharedPeriods = periods1.Intersect(periods2).OfType<string>().ToList();

        return Ok(new SiteComparisonDto
        {
            Site1 = s1.ToDetailDto(),
            Site2 = s2.ToDetailDto(),
            DistanceKm = distance,
            ChronologicalOverlapYears = overlapYears,
            IsContemporaneous = isContemporaneous,
            SharedCivilizations = sharedCivs,
            SharedPeriods = sharedPeriods
        });
    }
}
