using AocLib;

namespace _2023.Day14;

// 93102
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var map = input
            .SplitLines()
            .Select(_ => _.ToArray())
            .ToArray();

        var hash = new Dictionary<long, (long i, long v)>();

        const long iter = 1000000000;
        long start = 0;
        long end = 0;

        for (int i = 0; i < iter; i++)
        {
            MoveNorth(map);
            MoveWest(map);
            MoveSouth(map);
            MoveEast(map);

            var code = map.ComputeHash();
            if (hash.TryGetValue(code, out var v))
            {
                start = v.i;
                end = i;
                break;
            }

            hash[code] = (i, Compute(map));
        }

        var p1 = iter - start;
        var p2 = p1 % (end - start);
        var p3 = p2 + start - 1;

        return hash
            .Values
            .First(_ => _.i == p3)
            .v;
    }

    void MoveNorth(char[][] map)
    {
        var cur = new int[map[0].Length];

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
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
    }
    void MoveSouth(char[][] map)
    {
        var cur = new int[map[0].Length];
        Array.Fill(cur, map.Length - 1);

        for (int y = map.Length - 1; y >= 0; y--)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    cur[x] = y - 1;
                    continue;
                }

                if (map[y][x] == 'O')
                {
                    map[y][x] = '.';
                    map[cur[x]][x] = 'O';
                    cur[x]--;
                    continue;
                }
            }
        }

    }
    void MoveWest(char[][] map)
    {
        var cur = new int[map.Length];

        for (int x = 0; x < map[0].Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[y][x] == '#')
                {
                    cur[y] = x + 1;
                    continue;
                }

                if (map[y][x] == 'O')
                {
                    map[y][x] = '.';
                    map[y][cur[y]] = 'O';
                    cur[y]++;
                    continue;
                }
            }
        }

    }
    void MoveEast(char[][] map)
    {
        var cur = new int[map.Length];
        Array.Fill(cur, map[0].Length - 1);

        for (int x = map[0].Length - 1; x >= 0; x--)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[y][x] == '#')
                {
                    cur[y] = x - 1;
                    continue;
                }

                if (map[y][x] == 'O')
                {
                    map[y][x] = '.';
                    map[y][cur[y]] = 'O';
                    cur[y]--;
                    continue;
                }
            }
        }

    }

    void PrintMap(char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
    long Compute(char[][] map)
        => map.Select(_ => _.Where(c => c == 'O').Count())
            .WithIndex()
            .Sum(_ => (map.Length - _.Index) * _.Value);
}
