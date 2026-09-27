using System;

namespace ArchaeologicalTimeMachine.Domain.Common;

/// <summary>
/// Provides archaeological calendar calculations and BCE/CE formatting.
/// Historical dates are stored as signed integers:
/// -3000 = 3000 BCE, -1 = 1 BCE, 1 = 1 CE, 500 = 500 CE.
/// In ISO-8601 astronomical numbering, year 0 corresponds to 1 BCE.
/// </summary>
public static class TemporalHelper
{
    public static string FormatYear(int year)
    {
        if (year < 0)
        {
            return $"{Math.Abs(year)} BCE";
        }
        if (year == 0)
        {
            return "1 BCE";
        }
        return $"{year} CE";
    }

    public static string FormatYearSpan(int startYear, int endYear)
    {
        return $"{FormatYear(startYear)} – {FormatYear(endYear)}";
    }

    /// <summary>
    /// Checks if a target year falls within the site's active period [startYear, endYear].
    /// </summary>
    public static bool IsYearWithin(int targetYear, int startYear, int endYear)
    {
        return targetYear >= startYear && targetYear <= endYear;
    }

    /// <summary>
    /// Checks if two temporal intervals overlap.
    /// Interval 1: [s1, e1], Interval 2: [s2, e2].
    /// </summary>
    public static bool Overlaps(int start1, int end1, int start2, int end2)
    {
        return start1 <= end2 && end1 >= start2;
    }

    /// <summary>
    /// Computes the number of overlapping years between two sites or periods.
    /// Returns 0 if there is no overlap.
    /// </summary>
    public static int OverlapDuration(int start1, int end1, int start2, int end2)
    {
        int maxStart = Math.Max(start1, start2);
        int minEnd = Math.Min(end1, end2);
        return maxStart <= minEnd ? (minEnd - maxStart) : 0;
    }
}
