using AocLib;
using QuikGraph;
using System.Collections.Generic;

namespace AdventOfCode._2023.Day23;

public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var map = input.SplitLines();

        var start = new Point(map[0].IndexOf('.'), 0);
        var end = new Point(map[^1].IndexOf('.'), map.Length - 1);

        var dp = new int[map.Length, map[0].Length];
        dp[start.Y, start.X] = 1;

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map.Length; x++)
            {
                if (map[y][x] == '.')
                {
                    var neighbords = new Point(x, y)
                        .OrthogonalAdjacentPoints()
                        .Where(_ => _.InBounds(0, 0, map[0].Length - 1, map.Length - 1))
                        .Where(_ => map[_.Y][_.X] != '#');

                    int max = 0;
                    foreach (var n in neighbords)
                    {
                        max = Math.Max(max, dp[n.Y, n.X]);
                    }
                    
                    dp[y, x] = 1 + max;
                }

            }
        }

        return dp[end.Y, end.X];
    }
}
