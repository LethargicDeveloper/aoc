using System.Numerics;
using System.Text;
using MoreLinq;

namespace _2024.Day21;

[Answer(163920)]
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var numpad = """
                     789
                     456
                     123
                      0A
                     """.ToGrid<char>();

        var dirpad = """
                      ^A
                     <v>
                     """.ToGrid<char>();

        var numpadStart = numpad.Find('A');
        var dirpadStart = dirpad.Find('A');

        List<Point> GetPathByX(Point s, Point e) => [s, ..GetPointsBetweenByX(s, e), e];
        List<Point> GetPathByY(Point s, Point e) => [s, ..GetPointsBetweenByY(s, e), e];
        
        List<Point> GetDirections(List<Point> path, Point avoid)
        {
            var dirs = path.Zip(path.GetDirections()).ToList();
            var bad = dirs.FindIndex(d => d.First == avoid);
            if (bad != -1)
            {
                for (int i = 1; i <= dirs.Count / 2; i++)
                {
                    (dirs[i], dirs[^i]) = (dirs[^i], dirs[i]);
                }
            }

            return dirs.Select(d => d.Second).ToList();
        }

        List<Point> GetRobotPathByX(Point start, Point end, Point avoid) =>
            GetRobotPath(start, end, avoid, GetPathByX);
        
        List<Point> GetRobotPathByY(Point start, Point end, Point avoid) =>
            GetRobotPath(start, end, avoid, GetPathByY);
        
        List<Point> GetRobotPath(Point start, Point end, Point avoid, Func<Point, Point, List<Point>> getPath)
        {
            var path = getPath(start, end);
                
            var symbols = GetDirections(path, avoid)
                .Skip(1)
                .Select(dir => dir switch
                {
                    _ when dir == Point.Up => '^',
                    _ when dir == Point.Down => 'v',
                    _ when dir == Point.Left => '<',
                    _ when dir == Point.Right => '>',
                    _ => ' ',
                })
                .ToList();
                
            return symbols
                .Append('A')
                .Where(s => s != ' ')
                .Select(dirpad.Find)
                .ToList();
        }

        List<Point> GetDirPadRobotPath(Point start, List<Point> prevPath, Func<Point, Point, Point, List<Point>> getPath)
        {
            return new List<Point> { start }
                .Concat(prevPath)
                .Window(2)
                .SelectMany(w => getPath(w[0], w[1], (0, 0)))
                .ToList();
        }
        
        var codes = input.SplitLines();

        var shortestPaths = new List<(string Code, string Path)>();
        
        foreach (var code in codes)
        {
            var rob1pos = numpadStart;
            
            var sb = new StringBuilder();

            foreach (var num in code)
            {
                var end = numpad.Find(num);
                
                var rob2PathX = GetRobotPathByX(rob1pos, end, (0, 3));
                var rob2PathY = GetRobotPathByY(rob1pos, end, (0, 3));

                var rob3PathXX = GetDirPadRobotPath(dirpadStart, rob2PathX, GetRobotPathByX);
                var rob3PathXY = GetDirPadRobotPath(dirpadStart, rob2PathY, GetRobotPathByX);
                var rob3PathYX = GetDirPadRobotPath(dirpadStart, rob2PathX, GetRobotPathByY);
                var rob3PathYY = GetDirPadRobotPath(dirpadStart, rob2PathY, GetRobotPathByY);
                
                var rob4PathXXX = GetDirPadRobotPath(dirpadStart, rob3PathXX, GetRobotPathByX);
                var rob4PathXXY = GetDirPadRobotPath(dirpadStart, rob3PathXY, GetRobotPathByX);
                var rob4PathXYX = GetDirPadRobotPath(dirpadStart, rob3PathYX, GetRobotPathByX);
                var rob4PathXYY = GetDirPadRobotPath(dirpadStart, rob3PathYY, GetRobotPathByX);
                var rob4PathYXX = GetDirPadRobotPath(dirpadStart, rob3PathXX, GetRobotPathByY);
                var rob4PathYXY = GetDirPadRobotPath(dirpadStart, rob3PathXY, GetRobotPathByY);
                var rob4PathYYX = GetDirPadRobotPath(dirpadStart, rob3PathYX, GetRobotPathByY);
                var rob4PathYYY = GetDirPadRobotPath(dirpadStart, rob3PathYY, GetRobotPathByY);

                var robPath = new[]
                {
                    rob4PathXXX,
                    rob4PathXXY,
                    rob4PathXYX,
                    rob4PathXYY,
                    rob4PathYXX,
                    rob4PathYXY,
                    rob4PathYYX,
                    rob4PathYYY
                }.MinBy(p => p.Count);
                   
                rob1pos = end;

                var display = string.Join("", robPath.Select(p => dirpad[p]));
                sb.Append(display);
            }
            
            shortestPaths.Add((code, sb.ToString()));
        }

        // var answers = new Dictionary<string, string>()
        // {
        //     {"029A", "<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A"},
        //     {"980A", "<v<A>>^AAAvA^A<vA<AA>>^AvAA<^A>A<v<A>A>^AAAvA<^A>A<vA>^A<A>A"},
        //     {"179A", "<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A"},
        //     {"456A", "<v<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>A<v<A>A>^AAvA<^A>A"},
        //     {"379A", "<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A"}
        // };
        //
        // foreach (var (code, path) in shortestPaths)
        // {
        //     Console.WriteLine($"{code}:\t{path}");
        //     Console.WriteLine($"\t{answers[code]}");
        //     Console.WriteLine();
        // }
        
        return shortestPaths
            .Select(s => s.Path.Length * s.Code.ParseNumbers<int>()[0][0])
            .Sum();
    }
    
    public static IEnumerable<Point<T>> GetPointsBetweenByY<T>(Point<T> start, Point<T> end)
        where T : INumber<T>
    {
        T x = start.X;
        T y = start.Y;

        while (x != end.X || y != end.Y)
        {
            bool changed = y != end.Y;
            
            if (y < end.Y) y++;
            else if (y > end.Y) y--;

            if (!changed)
            {
                if (x < end.X) x++;
                else if (x > end.X) x--;
            }
            
            if ((x, y) == end)
                yield break;
            
            yield return new Point<T>(x, y);
        }
    }
    
    public static IEnumerable<Point<T>> GetPointsBetweenByX<T>(Point<T> start, Point<T> end)
        where T : INumber<T>
    {
        T x = start.X;
        T y = start.Y;
        
        while (x != end.X || y != end.Y)
        {
            bool changed = x != end.X;
            
            if (x < end.X) x++;
            else if (x > end.X) x--;
        
            if (!changed)
            {
                if (y < end.Y) y++;
                else if (y > end.Y) y--;
        
            }
            
            if ((x, y) == end)
                yield break;
            
            yield return new Point<T>(x, y);
        }
    }
}
