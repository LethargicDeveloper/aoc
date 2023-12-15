using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day22;

public partial class Part01 : PuzzleSolver<long>
{
    [GeneratedRegex("(\\d+)(.)?")]
    private static partial Regex NumberRegex();

    public override long Solve()
    {
        var (map, cmds) = Parse();
        var monkeyMap = new MonkeyMap(map, cmds);
        var password = monkeyMap.Solve();

        return password;
    }

    (Map, (int move, char rot)[]) Parse()
    {
        var inputs = this.input.SplitEmptyLines();
        var mapInputs = inputs[0].SplitLines();
        var cmdInputs = inputs[1];

        int FindMinY(int x)
        {
            for (int y = 0; y < mapInputs.Length; ++y)
            {
                if (x > mapInputs[y].Length - 1)
                    continue;

                if (mapInputs[y][x] != ' ')
                    return y;
            }

            throw new Exception("There's a blank column!");
        }

        int FindMaxY(int x)
        {
            for (int y = mapInputs.Length - 1; y > 0; --y)
            {
                if (x > mapInputs[y].Length - 1)
                    continue;

                if (mapInputs[y][x] != ' ')
                    return y;
            }

            throw new Exception("There's a blank column!");
        }

        var map = mapInputs
            .Select((v, y) => (
                v.Select((c, x) =>
                {
                    var list = v.ToList();
                    var minX = list.FindIndex(c => c != ' ');
                    var maxX = list.FindLastIndex(c => c != ' ');
                    var minY = FindMinY(x);
                    var maxY = FindMaxY(x);
                    return (minX, maxX, minY, maxY, c);
                }).ToArray()))
            .ToList();

        var cmds = NumberRegex()
            .Matches(cmdInputs)
            .Select(_ => (int.Parse(_.Groups[1].Value), _.Groups[2].Value.ElementAtOrDefault(0)))
            .ToArray();

        return (new Map(map), cmds);
    }

    class Map : List<(int minX, int maxX, int minY, int maxY, char token)[]>
    {
        public Map(List<(int minX, int maxX, int minY, int maxY, char token)[]> map)
        {
            this.AddRange(map);
        }
    }

    class MonkeyMap
    {
        const int Right = 0;
        const int Down = 1;
        const int Left = 2;
        const int Up = 3;

        static readonly Point[] dirMap = new[]
        {
        new Point(1, 0),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(0, -1)
    };

        readonly Map map;
        readonly (int move, char rot)[] cmds;

        public MonkeyMap(Map map, (int, char)[] cmds)
        {
            this.map = map;
            this.cmds = cmds;
        }

        public long Solve()
        {
            var (pos, rot) = GetEndingPos();

            return (1000 * (pos.Y + 1)) + (4 * (pos.X + 1)) + rot;
        }

        (Point, int) GetEndingPos()
        {
            var pos = GetStartingPos();
            var facing = Right;

            foreach (var cmd in cmds)
            {
                (pos, facing) = GetNextPos(pos, facing, cmd);
            }

            return (pos, facing);
        }

        (Point, int) GetNextPos(Point pos, int facing, (int move, char rot) cmd)
        {
            for (int i = 0; i < cmd.move; ++i)
            {
                (pos, var newFacing) = FindNextPos(pos, facing, cmd.rot);
                if (newFacing != facing)
                    return (pos, newFacing);

                facing = newFacing;
            }

            return (pos, GetNextFacing(facing, cmd.rot));
        }

        (Point, int) FindNextPos(Point pos, int facing, char rot)
        {
            var (minX, maxX, minY, maxY, _) = map[pos.Y][pos.X];

            var dir = dirMap[facing];
            var newPos = pos + dir;

            map[pos.Y][pos.X].token = facing switch
            {
                Right => '>',
                Down => 'V',
                Left => '<',
                Up => '^',
                _ => throw new Exception("Invalid facing.")
            };

            if (facing == Right || facing == Left)
            {
                var x = (newPos.X - minX).Mod(maxX - minX + 1) + minX;
                return (map[newPos.Y][x].token == '#')
                    ? (pos, GetNextFacing(facing, rot))
                    : (new Point(x, newPos.Y), facing);
            }

            if (facing == Up || facing == Down)
            {
                var y = (newPos.Y - minY).Mod(maxY - minY + 1) + minY;
                return (map[y][newPos.X].token == '#')
                    ? (pos, GetNextFacing(facing, rot))
                    : (new Point(newPos.X, y), facing);
            }

            throw new Exception("Invalid facing.");
        }

        static int GetNextFacing(int facing, char rot)
        {
            if (rot == '\0') return facing;

            facing += rot == 'R' ? 1 : -1;
            return facing.Mod(4);
        }

        Point GetStartingPos() => (map[0].Select(_ => _.token).ToList().FindIndex(_ => _ != ' '), 0);
    }
}
