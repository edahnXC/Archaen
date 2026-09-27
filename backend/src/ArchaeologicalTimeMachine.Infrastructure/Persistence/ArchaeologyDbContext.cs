using ArchaeologicalTimeMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Infrastructure.Persistence;

public class ArchaeologyDbContext : DbContext
{
    public ArchaeologyDbContext(DbContextOptions<ArchaeologyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Civilization> Civilizations => Set<Civilization>();
    public DbSet<HistoricalPeriod> HistoricalPeriods => Set<HistoricalPeriod>();
    public DbSet<SiteCivilization> SiteCivilizations => Set<SiteCivilization>();
    public DbSet<SitePeriod> SitePeriods => Set<SitePeriod>();
    public DbSet<Artefact> Artefacts => Set<Artefact>();
    public DbSet<Excavation> Excavations => Set<Excavation>();
    public DbSet<ExcavationLayer> ExcavationLayers => Set<ExcavationLayer>();
    public DbSet<Finding> Findings => Set<Finding>();
    public DbSet<Reference> References => Set<Reference>();
    public DbSet<SiteReference> SiteReferences => Set<SiteReference>();
    public DbSet<SiteRelationship> SiteRelationships => Set<SiteRelationship>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Site Configuration
        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
            entity.Property(s => s.Slug).IsRequired().HasMaxLength(150);
            entity.HasIndex(s => s.Slug).IsUnique();
            
            // Spatial & Temporal indexes for ultra-fast map queries
            entity.HasIndex(s => new { s.StartYear, s.EndYear });
            entity.HasIndex(s => new { s.Latitude, s.Longitude });
        });

        // Civilization Configuration
        modelBuilder.Entity<Civilization>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Slug).IsRequired().HasMaxLength(100);
            entity.HasIndex(c => c.Slug).IsUnique();
        });

        // Historical Period Configuration
        modelBuilder.Entity<HistoricalPeriod>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Slug).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Slug).IsUnique();
        });

        // SiteCivilization composite key
        modelBuilder.Entity<SiteCivilization>(entity =>
        {
            entity.HasKey(sc => new { sc.SiteId, sc.CivilizationId });

            entity.HasOne(sc => sc.Site)
                .WithMany(s => s.SiteCivilizations)
                .HasForeignKey(sc => sc.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.Civilization)
                .WithMany(c => c.SiteCivilizations)
                .HasForeignKey(sc => sc.CivilizationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SitePeriod composite key
        modelBuilder.Entity<SitePeriod>(entity =>
        {
            entity.HasKey(sp => new { sp.SiteId, sp.HistoricalPeriodId });

            entity.HasOne(sp => sp.Site)
                .WithMany(s => s.SitePeriods)
                .HasForeignKey(sp => sp.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sp => sp.HistoricalPeriod)
                .WithMany(p => p.SitePeriods)
                .HasForeignKey(sp => sp.HistoricalPeriodId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SiteReference composite key
        modelBuilder.Entity<SiteReference>(entity =>
        {
            entity.HasKey(sr => new { sr.SiteId, sr.ReferenceId });

            entity.HasOne(sr => sr.Site)
                .WithMany(s => s.SiteReferences)
                .HasForeignKey(sr => sr.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sr => sr.Reference)
                .WithMany(r => r.SiteReferences)
                .HasForeignKey(sr => sr.ReferenceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SiteRelationship Configuration
        modelBuilder.Entity<SiteRelationship>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.SourceSite)
                .WithMany(s => s.OutgoingRelationships)
                .HasForeignKey(r => r.SourceSiteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.TargetSite)
                .WithMany(s => s.IncomingRelationships)
                .HasForeignKey(r => r.TargetSiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Excavation & Layers Configuration
        modelBuilder.Entity<Excavation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Site)
                .WithMany(s => s.Excavations)
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExcavationLayer>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasOne(l => l.Excavation)
                .WithMany(e => e.Layers)
                .HasForeignKey(l => l.ExcavationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Finding>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.HasOne(f => f.ExcavationLayer)
                .WithMany(l => l.Findings)
                .HasForeignKey(f => f.ExcavationLayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Artefacts Configuration
        modelBuilder.Entity<Artefact>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(150);

            entity.HasOne(a => a.Site)
                .WithMany(s => s.Artefacts)
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
