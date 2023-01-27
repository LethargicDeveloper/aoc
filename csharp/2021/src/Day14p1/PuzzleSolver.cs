using BenchmarkDotNet.Attributes;
using System.Text;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input.SplitLines();
        var template = lines.First();
        var pairs = lines
            .Skip(1)
            .Select(_ => _.Split(" -> ") switch
            {
                var x => (pair: x[0], replace: x[1])
            });

        for (int i = 0; i < 10; ++i)
        {
            var list = new List<(string replace, int pos)>();
            for (int p = 0; p < template.Length - 1; ++p)
            {
                var len = p + 2;
                var polymer = template[p..len];
                var (pair, replace) = pairs.FirstOrDefault(_ => _.pair == polymer);
                if (pair != null)
                {
                    list.Add((replace, p));
                }
            }

            var sb = new StringBuilder(template);
            var counter = 1;
            foreach (var (replace, pos) in list.OrderBy(_ => _.pos))
            {
                sb.Insert(pos + counter, replace);
                counter++;
            }

            template = sb.ToString();
        }

        var g = template.GroupBy();
        var min = g.Min(_ => _.Count());
        var max = g.Max(_ => _.Count());
        return max - min;
    }
}