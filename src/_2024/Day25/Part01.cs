using MoreLinq;

namespace _2024.Day25;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var locks = input
            .SplitEmptyLines()
            .Where(x => x[0] == '#')
            .Select(s => s.SplitLines());
        
        var keys = input
            .SplitEmptyLines()
            .Where(x => x[0] == '.')
            .Select(s => s.SplitLines());

        var combos =
            from l in locks
            from k in keys
            select (l, k);

        var fits = combos
            .Select(c => c.l.Transpose().Select(r => r.Count(x => x == '.'))
                .Zip(c.k.Transpose().Select(r => r.Count(x => x == '#')), (l, k) => k <= l)
                .All(v => v))
            .Count(f => f);
        
        return fits;
    }
}
