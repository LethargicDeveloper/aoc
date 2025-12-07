using System.Text.RegularExpressions;
using AocLib;
using MoreLinq;

namespace _2015.Day05;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        bool IsNice(string str)
        {
            var r1 = Regex.Match(str, @"(..).*\1").Success;
            var r2 = Regex.Match(str, @"(.).\1").Success;
            return r1 && r2;
        }

        return input
            .SplitLines()
            .Where(IsNice)
            .Count();
    }
}
