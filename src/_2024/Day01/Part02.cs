using System.Runtime.ExceptionServices;
using AocLib;

namespace _2024.Day01;

public class Part02 : PuzzleSolver<long>
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

        var scores = new Dictionary<long, long>();
        foreach (var num in list1)
        {
            if (scores.ContainsKey(num))
                continue;

            scores[num] = list2.Count(x => x == num) * num;
        }

        var answer = list1
            .Select(x => scores[x])
            .Sum();
        
        return answer;
    }
}
