using System.Text.RegularExpressions;
using AocLib;

namespace _2015.Day05;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        bool IsNice(string str)
        {
            var r1 = str.Count("aeiou".Contains) >= 3;
            var r2 = Regex.Match(str, @"(.)\1").Success;
            var r3 = !new[]{"ab", "cd", "pq", "xy"}.Any(str.Contains);
            
            return r1 && r2 && r3;
        }

        return input
            .SplitLines()
            .Where(IsNice)
            .Count();
    }
}
