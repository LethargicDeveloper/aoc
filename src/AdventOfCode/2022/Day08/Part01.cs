using AocLib;

namespace AdventOfCode._2022.Day08;

public partial class Part01 : PuzzleSolver<long>
{
    static bool TreeIsVisible(int[][] forest, int x, int y)
    {
        bool isVisibleTop = Enumerable.Range(0, y)
            .All(y1 => forest[y1][x] < forest[y][x]);
        bool isVisibleLeft = Enumerable.Range(0, x)
            .All(x1 => forest[y][x1] < forest[y][x]);
        bool isVisibleBottom = Enumerable.Range(y + 1, forest.Length - y - 1)
            .All(y1 => forest[y1][x] < forest[y][x]);
        bool isVisibleRight = Enumerable.Range(x + 1, forest[0].Length - x - 1)
            .All(x1 => forest[y][x1] < forest[y][x]);

        return isVisibleTop || isVisibleBottom || isVisibleLeft || isVisibleRight;
    }

    public override long Solve()
    {
        var forest = input
            .SplitLines()
            .Select(_ => _.Select(c => c - '0').ToArray())
            .ToArray();

        return forest
            .Select((row, y) => row
                .Select((v, x) => TreeIsVisible(forest, x, y)))
            .SelectMany()
            .Count(_ => _ == true);
    }
}
