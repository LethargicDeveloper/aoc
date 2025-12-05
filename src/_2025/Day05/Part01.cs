using MoreLinq.Extensions;

namespace _2025.Day05;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var parts = input
            .SplitEmptyLines()
            .Select(l => l.SplitLines())
            .ToList();

        var ranges = parts[0]
            .Select(r =>
            {
                var nums = r.S('-').Select(long.Parse).Order().ToList();
                return new AocRange<long>(nums[0], nums[1]);
            })
            .MergeRanges();

        var count = parts[1]
            .Select(long.Parse)
            .Count(p => ranges.Any(r => r.Contains(p)));

        return count;
    }
}

