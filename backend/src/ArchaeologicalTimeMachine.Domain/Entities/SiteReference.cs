namespace ArchaeologicalTimeMachine.Domain.Entities;

public class SiteReference
{
    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public int ReferenceId { get; set; }
    public Reference? Reference { get; set; }

    public string? SpecificPagesOrPlates { get; set; }
}
