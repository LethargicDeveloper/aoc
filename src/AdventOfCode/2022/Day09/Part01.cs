using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day09;

public partial class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(" ") switch { var x => (dir: x[0], dist: int.Parse(x[1])) });

        var visited = new HashSet<(int, int)>();
        var knotPos = new Point[2];
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
