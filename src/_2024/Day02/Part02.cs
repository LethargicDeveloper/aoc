using AocLib;
using MoreLinq.Extensions;

namespace _2024.Day02;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var reports = input
            .SplitLines()
            .Select(x => x.Split(" ").Select(long.Parse).ToArray())
            .ToArray();

        long safe = 0;
        
        foreach (var report in reports)
        {
            var isSafe = IsSafe(report);
            if (isSafe) safe++;
        }
        
        return safe;
    }

    bool IsSafe(long[] report)
    {
        var valid = ValidRecord(report);
        if (valid) return true;
        
        for (int i = 0; i < report.Length; i++)
        {
            valid = ValidRecord(report.Where((_, ix) => ix != i).ToArray());
            if (valid) return true;
        }

        return false;
    }
    
    bool ValidRecord(long[] report)
    {
        var window = report.Window(2).Select(x => x[1] - x[0]).ToList();

        var noChange = window.Any(x => x == 0);
        var largeChange = window.Any(x => Math.Abs(x) > 3);
        var wrongDir = window.GroupBy(Math.Sign).Count() > 1;
        
        return (!noChange && !largeChange && !wrongDir);
    }
}
