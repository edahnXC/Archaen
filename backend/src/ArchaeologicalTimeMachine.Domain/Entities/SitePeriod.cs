namespace ArchaeologicalTimeMachine.Domain.Entities;

public class SitePeriod
{
    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public int HistoricalPeriodId { get; set; }
    public HistoricalPeriod? HistoricalPeriod { get; set; }
}
