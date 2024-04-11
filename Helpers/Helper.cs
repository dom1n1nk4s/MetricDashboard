using MetricDashboard.Data.Models;

namespace MetricDashboard.Helpers
{
    public static class Helper
    {
        public static List<(int from, int to)> GetOverCoveredRanges(IEnumerable<ColorRange> ranges)
        {
            var coveredPoints = new HashSet<int>();
            var overCoveredPoints = new List<(int from, int to)>();

            foreach (var range in ranges)
            {
                for (int i = range.From; i < range.To; i++)
                {
                    if (!coveredPoints.Add(i))
                    {
                        if (overCoveredPoints.Count == 0 || overCoveredPoints[^1].to != i)
                        {
                            overCoveredPoints.Add((i, i + 1));
                        }
                        else
                        {
                            overCoveredPoints[^1] = (overCoveredPoints[^1].from, i + 1);
                        }
                    }
                }
            }

            return overCoveredPoints;
        }
        public static List<(int from, int to)> GetNotCoveredRanges(IEnumerable<ColorRange> ranges, int start, int length)
        {
            // Initialize a boolean array to mark covered positions
            bool[] covered = new bool[length];

            // Mark covered positions based on the ranges provided
            foreach (var range in ranges)
            {
                for (int i = range.From; i < range.To && i < length; i++)
                {
                    covered[i] = true;
                }
            }

            // Initialize a list to store the not covered ranges
            var notCoveredRanges = new List<(int from, int to)>();

            // Iterate through the covered positions to find gaps
            int gapStart = -1;
            for (int i = start; i < length; i++)
            {
                if (!covered[i])
                {
                    if (gapStart == -1)
                    {
                        gapStart = i;
                    }
                }
                else
                {
                    if (gapStart != -1)
                    {
                        notCoveredRanges.Add((gapStart, i));
                        gapStart = -1;
                    }
                }
            }

            // If a gap is found at the end, add it to the list of not covered ranges
            if (gapStart != -1)
            {
                notCoveredRanges.Add((gapStart, length));
            }

            return notCoveredRanges;
        }
    }
}
