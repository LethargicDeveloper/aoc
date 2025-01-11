namespace _2024.Day22;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(long.Parse)
            .Select(num => num.NextSecret().Take(2000).Last())
            .Sum();
    }
}

