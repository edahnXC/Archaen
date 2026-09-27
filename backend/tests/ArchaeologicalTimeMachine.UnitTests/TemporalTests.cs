using ArchaeologicalTimeMachine.Domain.Common;
using FluentAssertions;
using Xunit;

namespace ArchaeologicalTimeMachine.UnitTests;

public class TemporalTests
{
    [Theory]
    [InlineData(-3000, "3000 BCE")]
    [InlineData(-2500, "2500 BCE")]
    [InlineData(-1, "1 BCE")]
    [InlineData(0, "1 BCE")]
    [InlineData(1, "1 CE")]
    [InlineData(79, "79 CE")]
    [InlineData(500, "500 CE")]
    public void FormatYear_ShouldCorrectlyFormatBceAndCeDates(int year, string expected)
    {
        // Act
        string result = TemporalHelper.FormatYear(year);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FormatYearSpan_ShouldFormatArchaeologicalInterval()
    {
        // Act
        string span1 = TemporalHelper.FormatYearSpan(-3000, -1500);
        string span2 = TemporalHelper.FormatYearSpan(-600, 79);

        // Assert
        span1.Should().Be("3000 BCE – 1500 BCE");
        span2.Should().Be("600 BCE – 79 CE");
    }

    [Fact]
    public void TimeMachine_PromptSpecificationRequirement_ShouldFilterSitesCorrectly()
    {
        // Prompt Section 29 Specification:
        // Given:
        // Site A: -3000 -> -2000
        // Site B: -1800 -> -1000
        int siteA_Start = -3000;
        int siteA_End = -2000;

        int siteB_Start = -1800;
        int siteB_End = -1000;

        // When timeline = -2500
        int timeline1 = -2500;
        bool siteA_At_2500BCE = TemporalHelper.IsYearWithin(timeline1, siteA_Start, siteA_End);
        bool siteB_At_2500BCE = TemporalHelper.IsYearWithin(timeline1, siteB_Start, siteB_End);

        // Expected: Site A visible, Site B hidden
        siteA_At_2500BCE.Should().BeTrue("Site A (-3000 to -2000) was active at 2500 BCE (-2500)");
        siteB_At_2500BCE.Should().BeFalse("Site B (-1800 to -1000) was not yet founded at 2500 BCE (-2500)");

        // When timeline = -1500
        int timeline2 = -1500;
        bool siteA_At_1500BCE = TemporalHelper.IsYearWithin(timeline2, siteA_Start, siteA_End);
        bool siteB_At_1500BCE = TemporalHelper.IsYearWithin(timeline2, siteB_Start, siteB_End);

        // Expected: Site A hidden, Site B visible
        siteA_At_1500BCE.Should().BeFalse("Site A (-3000 to -2000) had already ended by 1500 BCE (-1500)");
        siteB_At_1500BCE.Should().BeTrue("Site B (-1800 to -1000) was active at 1500 BCE (-1500)");
    }

    [Fact]
    public void Overlaps_ShouldDetectChronologicalContemporaneity()
    {
        // Dholavira (-3000 to -1500) and Lothal (-2400 to -1900)
        bool harappanCoexistence = TemporalHelper.Overlaps(-3000, -1500, -2400, -1900);
        int overlapYears = TemporalHelper.OverlapDuration(-3000, -1500, -2400, -1900);

        harappanCoexistence.Should().BeTrue();
        overlapYears.Should().Be(500); // from -2400 to -1900 is 500 years

        // Non-overlapping: Mature Harappan (-2500 to -1900) vs Pompeii (-600 to 79)
        bool nonOverlapping = TemporalHelper.Overlaps(-2500, -1900, -600, 79);
        nonOverlapping.Should().BeFalse();
        TemporalHelper.OverlapDuration(-2500, -1900, -600, 79).Should().Be(0);
    }
}
