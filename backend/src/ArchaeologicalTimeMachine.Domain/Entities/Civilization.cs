using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Civilization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#E06A3B";
    public string? PrimaryLanguage { get; set; }
    public string? ArchitecturalTradition { get; set; }

    public ICollection<SiteCivilization> SiteCivilizations { get; set; } = new List<SiteCivilization>();
}
