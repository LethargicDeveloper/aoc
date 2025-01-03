using AocLib;
using MoreLinq;

namespace _2023.Day21;

// 630129824772393
public class Part02 : PuzzleSolver<long>
{
    const int GridSize = 9;

    protected override long InternalSolve()
    {
        var lines = input.SplitLines();

        var mapTile = input
            .SplitLines()
            .Select(_ => _.ToCharArray())
            .ToArray();

        var mapRow = lines
            .Select(_ => _.Replace("S", ".").ToCharArray().Repeat(GridSize).ToArray())
            .ToList();

        var mapRowS = lines
            .Select(_ =>
            {
                var tiles = _.Replace("S", ".").ToCharArray().Repeat(GridSize / 2);
                return tiles.Concat(_).Concat(tiles).ToArray();
            })
            .ToList();

        var mapGroupList = new List<char[]>();
        for (int i = 0; i < GridSize / 2; i++)
            mapGroupList = [.. mapGroupList, .. mapRow];
        mapGroupList = [.. mapGroupList, .. mapRowS];
        for (int i = 0; i < GridSize / 2; i++)
            mapGroupList = [.. mapGroupList, .. mapRow];

        var map = mapGroupList.ToArray();

        var start = FindStartPos(map);
        var queue = new Queue<(Point pos, int steps)>();
        queue.Enqueue((start, 0));

        var tileSize = mapTile.Length;
        var initialSteps = tileSize / 2;

        var maxSteps = 26501365;
        var partialSteps = (maxSteps - initialSteps) % tileSize;
        var gridMiddle = (GridSize - 1) / 2;
        var endSteps = initialSteps + mapTile.Length * gridMiddle + partialSteps;

        var positions = new HashSet<Point>();

        while (queue.TryDequeue(out var loc))
        {
            var (pos, steps) = loc;

            if (steps == endSteps)
                continue;

            steps++;

            foreach (var n in pos
                .OrthogonalNeighbors()
                .Where(_ => _.InBounds(0, map.Length - 1)))
            {
                if (map[n.Y][n.X] == '.')
                    if (positions.Add(n))
                        queue.Enqueue((n, steps));
            }
        }

        var endPositions = positions
            .Where(_ => _.ManhattanDistance(start) % 2 == maxSteps % 2);

        if (maxSteps % 2 == 0)
            endPositions = endPositions.Append(start);

        var gridPositions = new List<List<Point>>();
        for (int y = 0; y < map.Length - 1; y += mapTile.Length)
            for (int x = 0; x < map.Length - 1; x += mapTile.Length)
            {
                var points = endPositions
                    .Where(_ =>
                        _.X >= x
                        && _.Y >= y
                        && _.X <= x + mapTile.Length - 1
                        && _.Y <= y + mapTile.Length - 1)
                    .ToList();

                gridPositions.Add(points);
            }

        long posCount(int x, int y) =>
            gridPositions[y * GridSize + x].Count();

        var left = posCount(0, gridMiddle);
        var right = posCount(GridSize - 1, gridMiddle);
        var up = posCount(gridMiddle, 0);
        var down = posCount(gridMiddle, GridSize - 1);

        var fullOdd = posCount(gridMiddle, gridMiddle);
        var fullEven = posCount(gridMiddle, gridMiddle - 1);

        var upLeftInner = posCount(gridMiddle - 1, 1);
        var upRightInner = posCount(gridMiddle + 1, 1);
        var downLeftInner = posCount(gridMiddle - 1, GridSize - 2);
        var downRightInner = posCount(gridMiddle + 1, GridSize - 2);

        var upLeftOuter = posCount(gridMiddle - 1, 0);
        var downLeftOuter = posCount(gridMiddle - 1, GridSize - 1);
        var upRightOuter = posCount(gridMiddle + 1, 0);
        var downRightOuter = posCount(gridMiddle + 1, GridSize - 1);

        var answer = up + down + left + right;

        var gridWidth = (long)Math.Ceiling(Math.Max(0, maxSteps - initialSteps) / (double)tileSize);

        var outerAngle = gridWidth;
        answer += outerAngle * upLeftOuter;
        answer += outerAngle * upRightOuter;
        answer += outerAngle * downLeftOuter;
        answer += outerAngle * downRightOuter;

        var innerAngle = outerAngle - 1;
        answer += innerAngle * upLeftInner;
        answer += innerAngle * upRightInner;
        answer += innerAngle * downLeftInner;
        answer += innerAngle * downRightInner;

        var fullSteps = gridWidth;
        var gridStepsEven = gridWidth % 2 == 0 ? fullSteps : fullSteps - 1;
        var gridStepsOdd = gridWidth % 2 == 0 ? fullSteps - 1 : fullSteps;

        answer += (long)Math.Pow(gridStepsEven, 2) * fullEven;
        answer += (long)Math.Pow(gridStepsOdd, 2) * fullOdd;

        Print(gridPositions);

        return answer;
    }

    void Print(List<List<Point>> gridPositions)
    {
        for (int i = 0; i < gridPositions.Count; i++)
        {
            int y = i / GridSize;
            int x = i % GridSize;

            if (x == 0 && y > 0) Console.WriteLine();

            Console.Write($"[{gridPositions[i].Count(),4}]");
        }
    }

    Point FindStartPos(char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[0].Length; x++)
                if (map[y][x] == 'S')
                    return (x, y);

        throw new Exception("Start position not found.");
    }
}
