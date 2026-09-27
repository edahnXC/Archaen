using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

/// <summary>
/// Represents an archaeological stratigraphic layer / occupational horizon.
/// LayerNumber 1 is the uppermost/most recent layer; increasing numbers represent deeper/older strata.
/// </summary>
public class ExcavationLayer
{
    public int Id { get; set; }
    public int ExcavationId { get; set; }
    public Excavation? Excavation { get; set; }

    public int LayerNumber { get; set; }
    public string LayerName { get; set; } = string.Empty;
    public double DepthMeters { get; set; }
    public string SoilComposition { get; set; } = string.Empty;
    public int EstimatedStartYear { get; set; }
    public int EstimatedEndYear { get; set; }
    public string CulturalAffiliation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Finding> Findings { get; set; } = new List<Finding>();
}
