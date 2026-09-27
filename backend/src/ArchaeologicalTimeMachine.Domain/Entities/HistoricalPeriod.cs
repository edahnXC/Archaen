using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

public class HistoricalPeriod
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Epoch { get; set; } = string.Empty; // Bronze Age, Iron Age, Classical Antiquity, Neolithic
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Description { get; set; } = string.Empty;

    public ICollection<SitePeriod> SitePeriods { get; set; } = new List<SitePeriod>();
}
