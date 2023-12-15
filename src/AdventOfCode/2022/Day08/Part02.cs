using AocLib;

namespace AdventOfCode._2022.Day08;

public partial class Part02 : PuzzleSolver<long>
{
    static long ScenicDistance(int[][] forest, int x, int y)
    {
        long bottom = Enumerable.Range(y + 1, forest.Length - y - 1)
            .TakeWhile(y1 => forest[y1][x] < forest[y][x])
            .Count();

        long top = Enumerable.Range(0, Math.Max(0, y - 1))
            .TakeWhile(y1 => forest[y - 1 - y1][x] < forest[y][x])
            .Count() + 1;

        long right = Enumerable.Range(x + 1, forest[0].Length - x - 1)
            .TakeWhile(x1 => forest[y][x1] < forest[y][x])
            .Count();

        long left = Enumerable.Range(0, Math.Max(0, x - 1))
            .TakeWhile(x1 => forest[y][x - 1 - x1] < forest[y][x])
            .Count() + 1;

        return top * bottom * left * right;
    }

    public override long Solve()
    {
        var forest = input
            .SplitLines()
            .Select(_ => _.Select(c => c - '0').ToArray())
            .ToArray();

        return forest
            .Select((row, y) => row
                .Select((v, x) => ScenicDistance(forest, x, y)))
            .SelectMany()
            .Max();
    }
}
