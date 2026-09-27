using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Api.DTOs;
using ArchaeologicalTimeMachine.Api.Services;
using ArchaeologicalTimeMachine.Domain.Common;
using ArchaeologicalTimeMachine.Domain.Entities;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SitesController : ControllerBase
{
    private readonly ArchaeologyDbContext _context;

    public SitesController(ArchaeologyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Searches and filters archaeological sites by temporal target year, chronological range, civilization, period, region, and keyword.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<SiteDto>>> GetSites(
        [FromQuery] int? year = null,
        [FromQuery] int? minYear = null,
        [FromQuery] int? maxYear = null,
        [FromQuery] int? civilizationId = null,
        [FromQuery] int? periodId = null,
        [FromQuery] string? region = null,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 200);

        IQueryable<Site> query = _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod);

        // Core Temporal Filter: Point in time (Signature Time Machine Feature)
        if (year.HasValue)
        {
            query = query.Where(s => s.StartYear <= year.Value && s.EndYear >= year.Value);
        }

        // Temporal Interval Overlap Filter
        if (minYear.HasValue && maxYear.HasValue)
        {
            query = query.Where(s => s.StartYear <= maxYear.Value && s.EndYear >= minYear.Value);
        }
        else if (minYear.HasValue)
        {
            query = query.Where(s => s.EndYear >= minYear.Value);
        }
        else if (maxYear.HasValue)
        {
            query = query.Where(s => s.StartYear <= maxYear.Value);
        }

        // Civilization Filter
        if (civilizationId.HasValue)
        {
            query = query.Where(s => s.SiteCivilizations.Any(sc => sc.CivilizationId == civilizationId.Value));
        }

        // Historical Period Filter
        if (periodId.HasValue)
        {
            query = query.Where(s => s.SitePeriods.Any(sp => sp.HistoricalPeriodId == periodId.Value));
        }

        // Region Filter
        if (!string.IsNullOrWhiteSpace(region))
        {
            string cleanRegion = region.Trim().ToLower();
            query = query.Where(s => s.Region.ToLower().Contains(cleanRegion) || s.Country.ToLower().Contains(cleanRegion));
        }

        // Text Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            string cleanSearch = search.Trim().ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(cleanSearch) ||
                                     (s.AncientName != null && s.AncientName.ToLower().Contains(cleanSearch)) ||
                                     s.Description.ToLower().Contains(cleanSearch) ||
                                     s.SiteType.ToLower().Contains(cleanSearch));
        }

        int totalCount = await query.CountAsync();

        var sites = await query
            .OrderBy(s => s.StartYear)
            .ThenBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = sites.Select(s => s.ToDto()).ToList();

        return Ok(new PagedResult<SiteDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>
    /// Gets full archaeological site details including artefacts, excavations, stratigraphy, references, and cross-site relationships.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SiteDetailDto>> GetSiteById(int id)
    {
        var site = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Include(s => s.Artefacts)
            .Include(s => s.Excavations).ThenInclude(e => e.Layers).ThenInclude(l => l.Findings)
            .Include(s => s.SiteReferences).ThenInclude(sr => sr.Reference)
            .Include(s => s.OutgoingRelationships).ThenInclude(r => r.TargetSite)
            .Include(s => s.IncomingRelationships).ThenInclude(r => r.SourceSite)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (site == null)
        {
            return NotFound(new { message = $"Archaeological site with ID {id} was not found." });
        }

        return Ok(site.ToDetailDto());
    }

    /// <summary>
    /// Gets archaeological site details by slug identifier.
    /// </summary>
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<SiteDetailDto>> GetSiteBySlug(string slug)
    {
        var site = await _context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.SitePeriods).ThenInclude(sp => sp.HistoricalPeriod)
            .Include(s => s.Artefacts)
            .Include(s => s.Excavations).ThenInclude(e => e.Layers).ThenInclude(l => l.Findings)
            .Include(s => s.SiteReferences).ThenInclude(sr => sr.Reference)
            .Include(s => s.OutgoingRelationships).ThenInclude(r => r.TargetSite)
            .Include(s => s.IncomingRelationships).ThenInclude(r => r.SourceSite)
            .FirstOrDefaultAsync(s => s.Slug == slug.ToLower());

        if (site == null)
        {
            return NotFound(new { message = $"Archaeological site with slug '{slug}' was not found." });
        }

        return Ok(site.ToDetailDto());
    }

    /// <summary>
    /// Geographic relationship query: discovers sites within a specified geodesic radius (km).
    /// </summary>
    [HttpGet("{id:int}/nearby")]
    public async Task<ActionResult<List<NearbySiteDto>>> GetNearbySites(int id, [FromQuery] double radiusKm = 1000, [FromQuery] int limit = 10)
    {
        var targetSite = await _context.Sites.FindAsync(id);
        if (targetSite == null)
        {
            return NotFound(new { message = $"Site {id} not found." });
        }

        var allOtherSites = await _context.Sites
            .Where(s => s.Id != id)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Slug,
                s.Region,
                s.Country,
                s.SiteType,
                s.StartYear,
                s.EndYear,
                s.Latitude,
                s.Longitude,
                s.ImageUrl
            })
            .ToListAsync();

        var nearby = allOtherSites
            .Select(s => new NearbySiteDto
            {
                Id = s.Id,
                Name = s.Name,
                Slug = s.Slug,
                Region = s.Region,
                Country = s.Country,
                SiteType = s.SiteType,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                DistanceKm = SpatialHelper.CalculateDistanceKm(targetSite.Latitude, targetSite.Longitude, s.Latitude, s.Longitude),
                ChronologicalSpan = TemporalHelper.FormatYearSpan(s.StartYear, s.EndYear),
                ImageUrl = s.ImageUrl
            })
            .Where(s => s.DistanceKm <= radiusKm)
            .OrderBy(s => s.DistanceKm)
            .Take(limit)
            .ToList();

        return Ok(nearby);
    }

    /// <summary>
    /// Temporal relationship query: discovers sites that were contemporaneous with the target site.
    /// </summary>
    [HttpGet("{id:int}/contemporaneous")]
    public async Task<ActionResult<List<ContemporaneousSiteDto>>> GetContemporaneousSites(int id, [FromQuery] int limit = 10)
    {
        var targetSite = await _context.Sites.FindAsync(id);
        if (targetSite == null)
        {
            return NotFound(new { message = $"Site {id} not found." });
        }

        var overlappingSites = await _context.Sites
            .Where(s => s.Id != id && s.StartYear <= targetSite.EndYear && s.EndYear >= targetSite.StartYear)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Slug,
                s.Region,
                s.Country,
                s.SiteType,
                s.StartYear,
                s.EndYear,
                s.Latitude,
                s.Longitude,
                s.ImageUrl
            })
            .ToListAsync();

        var contemporaneous = overlappingSites
            .Select(s => new ContemporaneousSiteDto
            {
                Id = s.Id,
                Name = s.Name,
                Slug = s.Slug,
                Region = s.Region,
                Country = s.Country,
                SiteType = s.SiteType,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                OverlapYears = TemporalHelper.OverlapDuration(targetSite.StartYear, targetSite.EndYear, s.StartYear, s.EndYear),
                ChronologicalSpan = TemporalHelper.FormatYearSpan(s.StartYear, s.EndYear),
                ImageUrl = s.ImageUrl
            })
            .OrderByDescending(s => s.OverlapYears)
            .Take(limit)
            .ToList();

        return Ok(contemporaneous);
    }

    /// <summary>
    /// Gets stratigraphic sequence and horizons for educational visualization (Phase 10).
    /// </summary>
    [HttpGet("{id:int}/stratigraphy")]
    public async Task<ActionResult<List<ExcavationLayerDto>>> GetSiteStratigraphy(int id)
    {
        var excavation = await _context.Excavations
            .Include(e => e.Layers).ThenInclude(l => l.Findings)
            .FirstOrDefaultAsync(e => e.SiteId == id);

        if (excavation == null)
        {
            return Ok(new List<ExcavationLayerDto>());
        }

        var layers = excavation.Layers
            .OrderBy(l => l.LayerNumber)
            .Select(l => new ExcavationLayerDto
            {
                Id = l.Id,
                LayerNumber = l.LayerNumber,
                LayerName = l.LayerName,
                DepthMeters = l.DepthMeters,
                SoilComposition = l.SoilComposition,
                EstimatedStartYear = l.EstimatedStartYear,
                EstimatedEndYear = l.EstimatedEndYear,
                ChronologicalSpan = TemporalHelper.FormatYearSpan(l.EstimatedStartYear, l.EstimatedEndYear),
                CulturalAffiliation = l.CulturalAffiliation,
                Description = l.Description,
                Findings = l.Findings.Select(f => new FindingDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    FindingType = f.FindingType,
                    Description = f.Description,
                    YearFound = f.YearFound
                }).ToList()
            })
            .ToList();

        return Ok(layers);
    }
}
