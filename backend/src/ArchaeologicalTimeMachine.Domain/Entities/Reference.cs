using System.Collections.Generic;

namespace ArchaeologicalTimeMachine.Domain.Entities;

public class Reference
{
    public int Id { get; set; }
    public string CitationKey { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string Title { get; set; } = string.Empty;
    public string JournalOrPublisher { get; set; } = string.Empty;
    public string? DoiOrIsbn { get; set; }
    public string? Url { get; set; }

    public ICollection<SiteReference> SiteReferences { get; set; } = new List<SiteReference>();
}
