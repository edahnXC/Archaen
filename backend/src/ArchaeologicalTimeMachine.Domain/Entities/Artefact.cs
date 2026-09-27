namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Artefact
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public string Name { get; set; } = string.Empty;
    public string ArtefactType { get; set; } = string.Empty; // Seal, Sculpture, Pottery, Jewellery, Tool, Tablet
    public string Material { get; set; } = string.Empty;     // Steatite, Bronze, Terracotta, Carnelian, Gold
    public int ApproximateYear { get; set; }
    public string Dimensions { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty; // e.g. National Museum New Delhi
    public string DiscoveryContext { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Model3DType { get; set; } // Identifier for 3D procedural/Three.js archaeological representation
}
