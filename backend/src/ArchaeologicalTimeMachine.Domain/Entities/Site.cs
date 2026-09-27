using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Site
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AncientName { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty; // Metropolis, Port, Citadel, Necropolis, Sanctuary, Temple City
    
    // Geographic coordinates in decimal degrees (WGS84)
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Astronomical year numbering: -3000 = 3000 BCE, 100 = 100 CE
    public int StartYear { get; set; }
    public int EndYear { get; set; }

    public string DatingPrecision { get; set; } = "Stratigraphic & Radiocarbon (14C)";
    public string DiscoveryInformation { get; set; } = string.Empty;
    public string ExcavationStatus { get; set; } = "Excavated & Conserved";
    public string WaterSource { get; set; } = string.Empty;
    public string ArchitecturalHighlights { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsUnescoWorldHeritage { get; set; }

    // Navigation properties
    public ICollection<SiteCivilization> SiteCivilizations { get; set; } = new List<SiteCivilization>();
    public ICollection<SitePeriod> SitePeriods { get; set; } = new List<SitePeriod>();
    public ICollection<Excavation> Excavations { get; set; } = new List<Excavation>();
    public ICollection<Artefact> Artefacts { get; set; } = new List<Artefact>();
    public ICollection<SiteReference> SiteReferences { get; set; } = new List<SiteReference>();
    public ICollection<SiteRelationship> OutgoingRelationships { get; set; } = new List<SiteRelationship>();
    public ICollection<SiteRelationship> IncomingRelationships { get; set; } = new List<SiteRelationship>();
}
