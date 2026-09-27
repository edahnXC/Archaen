namespace ArchaeologicalTimeMachine.Domain.Entities;

public class SiteCivilization
{
    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public int CivilizationId { get; set; }
    public Civilization? Civilization { get; set; }

    public bool IsPrimary { get; set; } = true;
}
