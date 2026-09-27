using ArchaeologicalTimeMachine.Domain.Common;
using FluentAssertions;
using Xunit;

namespace ArchaeologicalTimeMachine.UnitTests;

public class SpatialTests
{
    [Fact]
    public void CalculateDistanceKm_ShouldReturnZero_ForIdenticalCoordinates()
    {
        double distance = SpatialHelper.CalculateDistanceKm(23.886389, 70.217222, 23.886389, 70.217222);
        distance.Should().Be(0.0);
    }

    [Fact]
    public void CalculateDistanceKm_ShouldAccuratelyComputeDholaviraToLothalDistance()
    {
        // Dholavira (Khadir Bet, Kutch): 23.886389 N, 70.217222 E
        // Lothal (Saurashtra): 22.522222 N, 72.248611 E
        double distance = SpatialHelper.CalculateDistanceKm(23.886389, 70.217222, 22.522222, 72.248611);

        // Geodesic distance in Gujarat is approximately 257 km
        distance.Should().BeInRange(250.0, 265.0);
    }

    [Fact]
    public void CalculateDistanceKm_ShouldComputeMohenjoDaroToHarappaDistance()
    {
        // Mohenjo-daro: 27.329444 N, 68.138889 E
        // Harappa: 30.630000 N, 72.866944 E
        double distance = SpatialHelper.CalculateDistanceKm(27.329444, 68.138889, 30.630000, 72.866944);

        // Distance along the Indus drainage corridor is approx 580-600 km
        distance.Should().BeInRange(570.0, 610.0);
    }
}
