using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day22;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex("(\\d+)(.)?")]
    private static partial Regex NumberRegex();

    protected override long InternalSolve()
    {
        var (cube, cmds) = ParseCube();
        var monkeyCube = new MonkeyMapCube(cube, cmds);
        var password = monkeyCube.Solve();

        return password;
    }

    (Cube, (int move, char rot)[]) ParseCube()
    {
        var faces = new[]
        {
            new[] { 0, 1, 4},
            new[] { 0, 5, 0},
            new[] { 3, 6, 0},
            new[] { 2, 0, 0},
        };

        var inputs = this.input.SplitEmptyLines();
        var mapInputs = inputs[0].SplitLines();
        var cmdInputs = inputs[1];

        var size = mapInputs.Min(_ => _.Where(c => c != ' ').Count());
        var cube = new Cube(size, faces.Select(_ => _.ToList()).ToList());

        for (int y = 0; y < faces.Length; ++y)
        {
            for (int x = 0; x < faces[0].Length; ++x)
            {
                var index = faces[y][x];
                if (index == 0) continue;

                var face = new List<char[]>();
                for (int y1 = y * size; y1 < (y * size) + size; ++y1)
                {
                    var line = mapInputs[y1].Substring(x * size, size).ToArray();
                    face.Add(line);
                }

                cube[index] = face.ToArray();
            }
        }

        var cmds = NumberRegex()
            .Matches(cmdInputs)
            .Select(_ => (int.Parse(_.Groups[1].Value), _.Groups[2].Value.ElementAtOrDefault(0)))
            .ToArray();

        return (cube, cmds);
    }

    class Cube : Dictionary<int, char[][]>
    {
        public Cube(int size, List<List<int>> faces)
        {
            this.Size = size;
            this.Faces = faces;
        }

        public int Size { get; init; }
        public List<List<int>> Faces { get; init; } = new();
    }

    class MonkeyMapCube
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

        readonly Cube cube;
        readonly (int move, char rot)[] cmds;

        public MonkeyMapCube(Cube cube, (int, char)[] cmds)
        {
            this.cube = cube;
            this.cmds = cmds;
        }

        public long Solve()
        {
            var (p, facing) = GetEndingPos();
            var (pos, face) = p;

            var offset = new Point(0, 0);
            for (int y = 0; y < cube.Faces.Count; ++y)
                for (int x = 0; x < cube.Faces[0].Count; ++x)
                    if (cube.Faces[y][x] == face)
                    {
                        offset = (x, y);
                        break;
                    }

            pos = (pos.X + (cube.Size * offset.X), pos.Y + (cube.Size * offset.Y));

            return (1000 * (pos.Y + 1)) + (4 * (pos.X + 1)) + facing;
        }

        ((Point pos, int face), int facing) GetEndingPos()
        {
            var pos = GetStartingPos();
            var facing = Right;

            foreach (var cmd in cmds)
            {
                (pos, facing) = GetNextPos(pos, facing, cmd);
            }

            return (pos, facing);
        }

        ((Point, int), int) GetNextPos((Point, int) pos, int facing, (int move, char rot) cmd)
        {
            for (int i = 0; i < cmd.move; ++i)
            {
                (pos, facing, var hit) = FindNextPos(pos, facing, cmd.rot);
                if (hit) return (pos, facing);
            }

            return (pos, GetNextFacing(facing, cmd.rot));
        }

        ((Point, int), int, bool) FindNextPos((Point, int) p, int facing, char rot)
        {
            var max = cube.Size - 1;
            var (pos, face) = p;

            var dir = dirMap[facing];
            var newPos = pos + dir;
            var newFace = face;
            var newFacing = facing;

            if (facing == Right && newPos.X > max)
            {
                (newPos, newFace, newFacing) = face switch
                {
                    1 => ((0, (int)pos.Y), 4, Right),
                    2 => (((int)pos.Y, max), 6, Up),
                    3 => ((0, (int)pos.Y), 6, Right),
                    4 => ((max, max - (int)pos.Y), 6, Left),
                    5 => (((int)pos.Y, max), 4, Up),
                    6 => ((max, max - (int)pos.Y), 4, Left),
                    _ => throw new Exception("Invalid face")
                };
            }

            if (facing == Down && newPos.Y > max)
            {
                (newPos, newFace, newFacing) = face switch
                {
                    1 => (((int)pos.X, 0), 5, Down),
                    2 => (((int)pos.X, 0), 4, Down),
                    3 => (((int)pos.X, 0), 2, Down),
                    4 => ((max, (int)pos.X), 5, Left),
                    5 => (((int)pos.X, 0), 6, Down),
                    6 => ((max, (int)pos.X), 2, Left),
                    _ => throw new Exception("Invalid face")
                };
            }

            if (facing == Left && newPos.X < 0)
            {
                (newPos, newFace, newFacing) = face switch
                {
                    1 => ((0, max - (int)pos.Y), 3, Right),
                    2 => (((int)pos.Y, 0), 1, Down),
                    3 => ((0, max - (int)pos.Y), 1, Right),
                    4 => ((max, (int)pos.Y), 1, Left),
                    5 => (((int)pos.Y, 0), 3, Down),
                    6 => ((max, (int)pos.Y), 3, Left),
                    _ => throw new Exception("Invalid face")
                };
            }

            if (facing == Up && newPos.Y < 0)
            {
                (newPos, newFace, newFacing) = face switch
                {
                    1 => ((0, (int)pos.X), 2, Right),
                    2 => (((int)pos.X, max), 3, Up),
                    3 => ((0, (int)pos.X), 5, Right),
                    4 => (((int)pos.X, max), 2, Up),
                    5 => (((int)pos.X, max), 1, Up),
                    6 => (((int)pos.X, max), 5, Up),
                    _ => throw new Exception("Invalid face")
                };
            }

            var result = cube[newFace][newPos.Y][newPos.X] == '#'
                ? (p, GetNextFacing(facing, rot), true)
                : ((newPos, newFace), newFacing, false);

            return result;
        }

        static int GetNextFacing(int facing, char rot)
        {
            if (rot == '\0') return facing;

            facing += rot == 'R' ? 1 : -1;
            return MathEx.Mod(facing, 4);
        }

        (Point, int) GetStartingPos()
        {
            var face = cube.Faces[0].First(_ => _ > 0);
            return ((cube[face][0].ToList().FindIndex(_ => _ != ' '), 0), face);
        }
    }
}
