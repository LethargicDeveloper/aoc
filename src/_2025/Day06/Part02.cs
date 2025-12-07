using MoreLinq;

namespace _2025.Day06;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input.SplitLines();

        var cols = lines[^1]
            .WithIndex()
            .Where(x => x.Value != ' ')
            .ToList();

        return lines[..^1]
            .Select(x => x.S(cols.Select(c => c.Index).ToList()))
            .Transpose()
            .Select(x => x
                .SelectMany(s => s.Reverse().WithIndex())
                .GroupBy(s => s.Index)
                .Select(s => s.Select(c => c.Value).AsString())
                .Select(long.Parse))
            .Select((x, i) => cols[i].Value switch
            {
                '+' => x.Sum(),
                '*' => x.Product()
            })
            .Sum();
    }
}

