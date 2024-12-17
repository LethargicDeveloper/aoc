using MoreLinq.Extensions;

namespace _2024.Day01;

[Answer(20719933)]
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lists = input
            .ParseNumbers<int>()
            .Transpose()
            .ToCollections();

        var answer = lists[0]
            .ToDictionary(k => k, v => lists[1].Count(x => x == v) * v)
            .Select(x => x.Value)
            .Sum();

        return answer;
    }
}