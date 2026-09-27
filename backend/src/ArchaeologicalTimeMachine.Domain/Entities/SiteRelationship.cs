namespace ArchaeologicalTimeMachine.Domain.Entities;

public class SiteRelationship
{
    public int Id { get; set; }

    public int SourceSiteId { get; set; }
    public Site? SourceSite { get; set; }

    public int TargetSiteId { get; set; }
    public Site? TargetSite { get; set; }

    public string RelationshipType { get; set; } = string.Empty; // Trade Partner, Contemporaneous Metropolis, Sister Port, Regional Capital
    public string Description { get; set; } = string.Empty;
}
