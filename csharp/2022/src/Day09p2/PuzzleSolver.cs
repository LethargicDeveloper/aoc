using BenchmarkDotNet.Attributes;

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
        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(" ") switch { var x => (dir: x[0], dist: int.Parse(x[1])) });

        var visited = new HashSet<(int, int)>();
        var knotPos = new Point[10];
        visited.Add(knotPos[^1]);

        foreach (var cmd in cmds)
        {
            var (dir, dist) = cmd;
            for (int dx = 0; dx < dist; ++dx)
            {
                var headPos = knotPos[0];
                knotPos[0] = dir switch
                {
                    "U" => headPos.Up(),
                    "D" => headPos.Down(),
                    "L" => headPos.Left(),
                    _ => headPos.Right(),
                };

                for (int i = 1; i < knotPos.Length; ++i)
                {
                    var knot = knotPos[i];
                    var prevKnot = knotPos[i - 1];

                    if (Math.Abs(prevKnot.X - knot.X) > 1 || Math.Abs(prevKnot.Y - knot.Y) > 1)
                    {
                        knotPos[i] = knot.MoveToward(prevKnot);
                    }
                }

                visited.Add(knotPos[^1]);
            }
        }

        return visited.Count;
    }
}