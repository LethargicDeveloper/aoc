using AocLib;

namespace _2023.Day14;

// 109385
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var map = input
            .SplitLines()
            .Select(_ => _.ToArray())
            .ToArray();

        var cur = new int[map[0].Length];

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map.Length; x++)
            {
                if (map[y][x] == '#')
                {
                    cur[x] = y + 1;
                    continue;
                }

                if (map[y][x] == 'O')
                {
                    map[y][x] = '.';
                    map[cur[x]][x] = 'O';
                    cur[x]++;
                    continue;
                }
            }
        }

        return map
            .Select(_ => _.Where(c => c == 'O').Count())
            .WithIndex()
            .Sum(_ => (map.Length - _.Index) * _.Value);
    }
}
