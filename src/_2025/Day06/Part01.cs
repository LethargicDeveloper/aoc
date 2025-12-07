using System.Globalization;
using MoreLinq.Extensions;

namespace _2025.Day06;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(x => x.S(' '))
            .Transpose()
            .Select(x => x.ToList())
            .Select(x => (Nums: x[..^1].Select(long.Parse), Op: x[^1]))
            .Select(x => x.Op switch
            {
                "+" => x.Nums.Sum(),
                "*" => x.Nums.Product()
            })
            .Sum();
    }
}

