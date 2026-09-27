using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Domain.Common;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using ArchaeologicalTimeMachine.Infrastructure.Seed;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ArchaeologicalTimeMachine.UnitTests;

public class ArchaeologyDatabaseTests
{
    [Fact]
    public async Task SeededDatabase_ShouldContainVerifiedArchaeologicalSites()
    {
        // Arrange
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ArchaeologyDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ArchaeologyDbContext(options);
        await DatabaseSeeder.SeedAsync(context);

        // Act
        var sites = await context.Sites
            .Include(s => s.SiteCivilizations).ThenInclude(sc => sc.Civilization)
            .Include(s => s.Artefacts)
            .Include(s => s.Excavations).ThenInclude(e => e.Layers)
            .ToListAsync();

        // Assert
        sites.Should().NotBeEmpty();
        sites.Count.Should().BeGreaterThanOrEqualTo(8);

        // Verify Dholavira
        var dholavira = sites.FirstOrDefault(s => s.Slug == "dholavira");
        dholavira.Should().NotBeNull();
        dholavira!.StartYear.Should().Be(-3000);
        dholavira.EndYear.Should().Be(-1500);
        dholavira.IsUnescoWorldHeritage.Should().BeTrue();
        dholavira.Artefacts.Should().Contain(a => a.Name.Contains("Signboard"));
        dholavira.Excavations.Should().NotBeEmpty();
        dholavira.Excavations.First().Layers.Should().HaveCount(3);
    }

    [Fact]
    public async Task TemporalQuery_At2500BCE_ShouldReturnBronzeAgeSites()
    {
        // Arrange
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ArchaeologyDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ArchaeologyDbContext(options);
        await DatabaseSeeder.SeedAsync(context);

        // Act: Target year 2500 BCE (-2500)
        int targetYear = -2500;
        var activeSites = await context.Sites
            .Where(s => s.StartYear <= targetYear && s.EndYear >= targetYear)
            .ToListAsync();

        // Assert
        // Dholavira (-3000 to -1500), Mohenjo-daro (-2500 to -1900), Harappa (-3300 to -1300), Ur (-3800 to -500), Giza (-2580 to -2150) should be active
        activeSites.Should().Contain(s => s.Slug == "dholavira");
        activeSites.Should().Contain(s => s.Slug == "mohenjo-daro");
        activeSites.Should().Contain(s => s.Slug == "harappa");
        activeSites.Should().Contain(s => s.Slug == "ur-tell-el-mukayyar");
        activeSites.Should().Contain(s => s.Slug == "giza-necropolis");

        // Pompeii (-600 to 79 CE) should NOT be active at 2500 BCE
        activeSites.Should().NotContain(s => s.Slug == "pompeii");
    }

    [Fact]
    public async Task TemporalQuery_At70CE_ShouldReturnRomanSitesOnly()
    {
        // Arrange
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ArchaeologyDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ArchaeologyDbContext(options);
        await DatabaseSeeder.SeedAsync(context);

        // Act: Target year 70 CE
        int targetYear = 70;
        var activeSites = await context.Sites
            .Where(s => s.StartYear <= targetYear && s.EndYear >= targetYear)
            .ToListAsync();

        // Assert: Pompeii (-600 to 79 CE) is active; Bronze age Indus sites are long abandoned
        activeSites.Should().Contain(s => s.Slug == "pompeii");
        activeSites.Should().NotContain(s => s.Slug == "dholavira");
        activeSites.Should().NotContain(s => s.Slug == "mohenjo-daro");
        activeSites.Should().NotContain(s => s.Slug == "giza-necropolis");
    }
}
