using MoreLinq.Extensions;

namespace _2024.Day02;

[Answer(589)]
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var safe = input
            .ParseNumbers<long>()
            .Count(IsSafe);
       
        return safe;
    }

    bool IsSafe(List<long> report)
    {
        return Enumerable
            .Range(0, report.Count)
            .Select(i => report.Where((_, ix) => ix != i).ToList())
            .Any(ValidRecord);
    }
    
    bool ValidRecord(List<long> report)
    {
        var window = report.Window(2).Select(x => x[1] - x[0]).ToList();

        var noChange = window.Any(x => x == 0);
        var largeChange = window.Any(x => Math.Abs(x) > 3);
        var wrongDir = window.GroupBy(Math.Sign).Count() > 1;
        
        return (!noChange && !largeChange && !wrongDir);
    }
}
