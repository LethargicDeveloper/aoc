namespace _2025.Day12;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitEmptyLines()[^1]
            .SplitLines()
            .Select(line => line.ParseNumbers<int>()[0].ToList())
            .Count(r => r[0] * r[1] >= r[2..].Sum() * 9);
    }
}