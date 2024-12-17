using MoreLinq.Extensions;

namespace _2024.Day01;

[Answer(2164381)]
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var answer = input
            .ParseNumbers<int>()
            .Transpose()
            .Select(x => x.Order())
            .Transpose()
            .Select(x => x.ToList())
            .Select(x => Math.Abs(x[0] - x[1]))
            .Sum();
        
        return answer;
    }
}