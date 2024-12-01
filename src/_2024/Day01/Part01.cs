using AocLib;
using MoreLinq;

namespace _2024.Day01;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var (list1, list2) = input
            .SplitLines()
            .Select(x => x.Split("   "))
            .Aggregate((List1: new List<long>(), List2: new List<long>()), (acc, cur) =>
            {
                acc.List1.Add(int.Parse(cur[0]));
                acc.List2.Add(int.Parse(cur[1]));
                return acc;
            });

        var answer = list1
            .OrderBy()
            .Zip(list2.OrderBy(),
                (a, b) => Math.Abs(a - b))
            .Sum();
        
        return answer;
    }
}
