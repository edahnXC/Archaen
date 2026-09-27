namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Finding
{
    public int Id { get; set; }
    public int ExcavationLayerId { get; set; }
    public ExcavationLayer? ExcavationLayer { get; set; }

    public string Name { get; set; } = string.Empty;
    public string FindingType { get; set; } = string.Empty; // Ceramic, Faunal, Lithic, Metal, Structural
    public string Description { get; set; } = string.Empty;
    public int YearFound { get; set; }
}
