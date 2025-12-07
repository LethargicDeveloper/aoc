using AocLib;

namespace _2015.Day01;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input.Sum(x => x == '(' ? 1 : -1);
    }
}
