using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Excavation
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public string ExpeditionName { get; set; } = string.Empty;
    public string LeadArchaeologist { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string Organization { get; set; } = string.Empty; // e.g. "Archaeological Survey of India"
    public string Summary { get; set; } = string.Empty;

    public ICollection<ExcavationLayer> Layers { get; set; } = new List<ExcavationLayer>();
}
