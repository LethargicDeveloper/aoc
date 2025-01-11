using MoreLinq;

namespace _2024.Day22;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(long.Parse)
            .SelectMany(num => num
                .NextSecret()
                .Take(2000)
                .Prepend(num)
                .Select(n => (int)char.GetNumericValue($"{n}"[^1]))
                .Window(2)
                .Select(n => (Value: n[1], Change: n[1] - n[0]))
                .Window(4)
                .Select(n => (n[^1].Value, Window: (n[0].Change, n[1].Change, n[2].Change, n[3].Change)))
                .GroupBy(g => g.Window)
                .Select(g => g.First()))
            .GroupBy(g => g.Window)
            .Select(g => g.Sum(v => v.Value))
            .Max();
    }
}
