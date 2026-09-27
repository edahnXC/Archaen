using System.Linq;
using ArchaeologicalTimeMachine.Api.DTOs;
using ArchaeologicalTimeMachine.Domain.Common;
using ArchaeologicalTimeMachine.Domain.Entities;

namespace ArchaeologicalTimeMachine.Api.Services;

public static class DtoMappingExtensions
{
    public static SiteDto ToDto(this Site s)
    {
        return new SiteDto
        {
            Id = s.Id,
            Name = s.Name,
            AncientName = s.AncientName,
            Slug = s.Slug,
            Description = s.Description,
            Region = s.Region,
            Country = s.Country,
            SiteType = s.SiteType,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            StartYear = s.StartYear,
            EndYear = s.EndYear,
            StartYearFormatted = TemporalHelper.FormatYear(s.StartYear),
            EndYearFormatted = TemporalHelper.FormatYear(s.EndYear),
            ChronologicalSpan = TemporalHelper.FormatYearSpan(s.StartYear, s.EndYear),
            DatingPrecision = s.DatingPrecision,
            ExcavationStatus = s.ExcavationStatus,
            WaterSource = s.WaterSource,
            ArchitecturalHighlights = s.ArchitecturalHighlights,
            ImageUrl = s.ImageUrl,
            IsUnescoWorldHeritage = s.IsUnescoWorldHeritage,
            Civilizations = s.SiteCivilizations?.Select(sc => new CivilizationSummaryDto
            {
                Id = sc.CivilizationId,
                Name = sc.Civilization?.Name ?? string.Empty,
                Slug = sc.Civilization?.Slug ?? string.Empty,
                ColorHex = sc.Civilization?.ColorHex ?? "#E06A3B",
                IsPrimary = sc.IsPrimary
            }).ToList() ?? new(),
            Periods = s.SitePeriods?.Select(sp => new PeriodSummaryDto
            {
                Id = sp.HistoricalPeriodId,
                Name = sp.HistoricalPeriod?.Name ?? string.Empty,
                Slug = sp.HistoricalPeriod?.Slug ?? string.Empty,
                Epoch = sp.HistoricalPeriod?.Epoch ?? string.Empty
            }).ToList() ?? new()
        };
    }

    public static SiteDetailDto ToDetailDto(this Site s)
    {
        var dto = new SiteDetailDto
        {
            Id = s.Id,
            Name = s.Name,
            AncientName = s.AncientName,
            Slug = s.Slug,
            Description = s.Description,
            Region = s.Region,
            Country = s.Country,
            SiteType = s.SiteType,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            StartYear = s.StartYear,
            EndYear = s.EndYear,
            StartYearFormatted = TemporalHelper.FormatYear(s.StartYear),
            EndYearFormatted = TemporalHelper.FormatYear(s.EndYear),
            ChronologicalSpan = TemporalHelper.FormatYearSpan(s.StartYear, s.EndYear),
            DatingPrecision = s.DatingPrecision,
            ExcavationStatus = s.ExcavationStatus,
            WaterSource = s.WaterSource,
            ArchitecturalHighlights = s.ArchitecturalHighlights,
            ImageUrl = s.ImageUrl,
            IsUnescoWorldHeritage = s.IsUnescoWorldHeritage,
            DiscoveryInformation = s.DiscoveryInformation,
            Civilizations = s.SiteCivilizations?.Select(sc => new CivilizationSummaryDto
            {
                Id = sc.CivilizationId,
                Name = sc.Civilization?.Name ?? string.Empty,
                Slug = sc.Civilization?.Slug ?? string.Empty,
                ColorHex = sc.Civilization?.ColorHex ?? "#E06A3B",
                IsPrimary = sc.IsPrimary
            }).ToList() ?? new(),
            Periods = s.SitePeriods?.Select(sp => new PeriodSummaryDto
            {
                Id = sp.HistoricalPeriodId,
                Name = sp.HistoricalPeriod?.Name ?? string.Empty,
                Slug = sp.HistoricalPeriod?.Slug ?? string.Empty,
                Epoch = sp.HistoricalPeriod?.Epoch ?? string.Empty
            }).ToList() ?? new(),
            Artefacts = s.Artefacts?.Select(a => a.ToDto()).ToList() ?? new(),
            Excavations = s.Excavations?.Select(e => new ExcavationDto
            {
                Id = e.Id,
                ExpeditionName = e.ExpeditionName,
                LeadArchaeologist = e.LeadArchaeologist,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                Organization = e.Organization,
                Summary = e.Summary,
                Layers = e.Layers?.OrderBy(l => l.LayerNumber).Select(l => new ExcavationLayerDto
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
                    Findings = l.Findings?.Select(f => new FindingDto
                    {
                        Id = f.Id,
                        Name = f.Name,
                        FindingType = f.FindingType,
                        Description = f.Description,
                        YearFound = f.YearFound
                    }).ToList() ?? new()
                }).ToList() ?? new()
            }).ToList() ?? new(),
            References = s.SiteReferences?.Select(sr => new ReferenceDto
            {
                Id = sr.ReferenceId,
                CitationKey = sr.Reference?.CitationKey ?? string.Empty,
                Authors = sr.Reference?.Authors ?? string.Empty,
                PublicationYear = sr.Reference?.PublicationYear ?? 0,
                Title = sr.Reference?.Title ?? string.Empty,
                JournalOrPublisher = sr.Reference?.JournalOrPublisher ?? string.Empty,
                DoiOrIsbn = sr.Reference?.DoiOrIsbn,
                Url = sr.Reference?.Url,
                SpecificPagesOrPlates = sr.SpecificPagesOrPlates
            }).ToList() ?? new(),
            OutgoingRelationships = s.OutgoingRelationships?.Select(r => new SiteRelationshipDto
            {
                Id = r.Id,
                RelatedSiteId = r.TargetSiteId,
                RelatedSiteName = r.TargetSite?.Name ?? string.Empty,
                RelatedSiteSlug = r.TargetSite?.Slug ?? string.Empty,
                RelationshipType = r.RelationshipType,
                Description = r.Description
            }).ToList() ?? new(),
            IncomingRelationships = s.IncomingRelationships?.Select(r => new SiteRelationshipDto
            {
                Id = r.Id,
                RelatedSiteId = r.SourceSiteId,
                RelatedSiteName = r.SourceSite?.Name ?? string.Empty,
                RelatedSiteSlug = r.SourceSite?.Slug ?? string.Empty,
                RelationshipType = r.RelationshipType,
                Description = r.Description
            }).ToList() ?? new()
        };

        return dto;
    }

    public static ArtefactDto ToDto(this Artefact a)
    {
        return new ArtefactDto
        {
            Id = a.Id,
            SiteId = a.SiteId,
            SiteName = a.Site?.Name ?? string.Empty,
            Name = a.Name,
            ArtefactType = a.ArtefactType,
            Material = a.Material,
            ApproximateYear = a.ApproximateYear,
            ApproximateYearFormatted = TemporalHelper.FormatYear(a.ApproximateYear),
            Dimensions = a.Dimensions,
            Description = a.Description,
            CurrentLocation = a.CurrentLocation,
            DiscoveryContext = a.DiscoveryContext,
            ImageUrl = a.ImageUrl,
            Model3DType = a.Model3DType
        };
    }

    public static CivilizationDto ToDto(this Civilization c, int siteCount = 0)
    {
        return new CivilizationDto
        {
            Id = c.Id,
            Name = c.Name,
            Slug = c.Slug,
            Region = c.Region,
            StartYear = c.StartYear,
            EndYear = c.EndYear,
            ChronologicalSpan = TemporalHelper.FormatYearSpan(c.StartYear, c.EndYear),
            Description = c.Description,
            ColorHex = c.ColorHex,
            PrimaryLanguage = c.PrimaryLanguage,
            ArchitecturalTradition = c.ArchitecturalTradition,
            SiteCount = siteCount
        };
    }

    public static HistoricalPeriodDto ToDto(this HistoricalPeriod p, int siteCount = 0)
    {
        return new HistoricalPeriodDto
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            Epoch = p.Epoch,
            StartYear = p.StartYear,
            EndYear = p.EndYear,
            ChronologicalSpan = TemporalHelper.FormatYearSpan(p.StartYear, p.EndYear),
            Description = p.Description,
            SiteCount = siteCount
        };
    }
}
