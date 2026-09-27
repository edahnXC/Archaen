using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Api.DTOs;

public class SiteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AncientName { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string StartYearFormatted { get; set; } = string.Empty;
    public string EndYearFormatted { get; set; } = string.Empty;
    public string ChronologicalSpan { get; set; } = string.Empty;
    public string DatingPrecision { get; set; } = string.Empty;
    public string ExcavationStatus { get; set; } = string.Empty;
    public string WaterSource { get; set; } = string.Empty;
    public string ArchitecturalHighlights { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsUnescoWorldHeritage { get; set; }

    public List<CivilizationSummaryDto> Civilizations { get; set; } = new();
    public List<PeriodSummaryDto> Periods { get; set; } = new();
}

public class CivilizationSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public class PeriodSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Epoch { get; set; } = string.Empty;
}

public class SiteDetailDto : SiteDto
{
    public string DiscoveryInformation { get; set; } = string.Empty;
    public List<ArtefactDto> Artefacts { get; set; } = new();
    public List<ExcavationDto> Excavations { get; set; } = new();
    public List<ReferenceDto> References { get; set; } = new();
    public List<SiteRelationshipDto> OutgoingRelationships { get; set; } = new();
    public List<SiteRelationshipDto> IncomingRelationships { get; set; } = new();
}

public class NearbySiteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty;
    public double DistanceKm { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ChronologicalSpan { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class ContemporaneousSiteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty;
    public int OverlapYears { get; set; }
    public string ChronologicalSpan { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ImageUrl { get; set; }
}

public class ArtefactDto
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ArtefactType { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public int ApproximateYear { get; set; }
    public string ApproximateYearFormatted { get; set; } = string.Empty;
    public string Dimensions { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public string DiscoveryContext { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Model3DType { get; set; }
}

public class ExcavationDto
{
    public int Id { get; set; }
    public string ExpeditionName { get; set; } = string.Empty;
    public string LeadArchaeologist { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string Organization { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<ExcavationLayerDto> Layers { get; set; } = new();
}

public class ExcavationLayerDto
{
    public int Id { get; set; }
    public int LayerNumber { get; set; }
    public string LayerName { get; set; } = string.Empty;
    public double DepthMeters { get; set; }
    public string SoilComposition { get; set; } = string.Empty;
    public int EstimatedStartYear { get; set; }
    public int EstimatedEndYear { get; set; }
    public string ChronologicalSpan { get; set; } = string.Empty;
    public string CulturalAffiliation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<FindingDto> Findings { get; set; } = new();
}

public class FindingDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FindingType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int YearFound { get; set; }
}

public class ReferenceDto
{
    public int Id { get; set; }
    public string CitationKey { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string Title { get; set; } = string.Empty;
    public string JournalOrPublisher { get; set; } = string.Empty;
    public string? DoiOrIsbn { get; set; }
    public string? Url { get; set; }
    public string? SpecificPagesOrPlates { get; set; }
}

public class SiteRelationshipDto
{
    public int Id { get; set; }
    public int RelatedSiteId { get; set; }
    public string RelatedSiteName { get; set; } = string.Empty;
    public string RelatedSiteSlug { get; set; } = string.Empty;
    public string RelationshipType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CivilizationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string ChronologicalSpan { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public string? PrimaryLanguage { get; set; }
    public string? ArchitecturalTradition { get; set; }
    public int SiteCount { get; set; }
}

public class HistoricalPeriodDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Epoch { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string ChronologicalSpan { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SiteCount { get; set; }
}

public class SiteComparisonDto
{
    public SiteDetailDto Site1 { get; set; } = default!;
    public SiteDetailDto Site2 { get; set; } = default!;
    public double DistanceKm { get; set; }
    public int ChronologicalOverlapYears { get; set; }
    public bool IsContemporaneous { get; set; }
    public List<string> SharedCivilizations { get; set; } = new();
    public List<string> SharedPeriods { get; set; } = new();
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
}
