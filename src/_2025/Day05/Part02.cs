using MoreLinq.Extensions;

namespace _2025.Day05;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var parts = input
            .SplitEmptyLines()
            .Select(l => l.SplitLines())
            .ToList();

        var count = parts[0]
            .Select(r =>
            {
                var nums = r.S('-').Select(long.Parse).Order().ToList();
                return new AocRange<long>(nums[0], nums[1]);
            })
            .MergeRanges()
            .Sum(r => r.Count);
        
        return count;
    }
}

