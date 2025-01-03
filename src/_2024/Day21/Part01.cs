using System.Text;
using MoreLinq;

namespace _2024.Day21;

// < 169132
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

        List<Point> GetPath(Point s, Point e) => [s, ..Point.GetPointsBetween(s, e), e];
        
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

        List<Point> GetRobotPath(Point start, Point end, Point avoid)
        {
            var path = GetPath(start, end);
                
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
        
        var codes = input.SplitLines();

        var shortestPaths = new List<(string Code, string Path)>();
        
        foreach (var code in codes)
        {
            var rob1pos = numpadStart;
            
            var sb = new StringBuilder();

            foreach (var num in code)
            {
                var end = numpad.Find(num);

                var rob2Path = GetRobotPath(rob1pos, end, (0, 3));
                
                var rob3Path = new List<Point> { dirpadStart }
                    .Concat(rob2Path)
                    .Window(2)
                    .SelectMany(w => GetRobotPath(w[0], w[1], (0, 0)));
                
                var rob4Path = new List<Point> { dirpadStart }
                    .Concat(rob3Path)
                    .Window(2)
                    .SelectMany(w => GetRobotPath(w[0], w[1], (0, 0)));

                rob1pos = end;

                var display = string.Join("", rob4Path.Select(p => dirpad[p]));
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
}

/*
         <vA<AA^>>A<A>vAA^Av<<A^>>AvA^Av<<A^>>AAvA<A>^A<A>Av<<A>A>^AAAvA<^A>A
   029A: <vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A
   
         v<<A^>>AAAvA^A<vA<AA^>>A<A>vAA^Av<<A>A>^AAAvA<^A>A<vA>^A<A>A
   980A: <v<A>>^AAAvA^A<vA<AA>>^AvAA<^A>A<v<A>A>^AAAvA<^A>A<vA>^A<A>A
   
         <<vAA>A>^AAvA<^A>AvA^A<<vA>>^AvA^A<vA>^A<A>A<<vA>A>^AAAvA<^A>A
   179A: <v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A
   
         <<vAA>A>^AAvA<^A>AAvA^A<vA>^A<A>A<vA>^A<A>A<<vA>A>^AAvA<^A>A
   456A: <v<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>A<v<A>A>^AAvA<^A>A
   
         <<vA>>^AvA^A<<vAA>A>^AAvA<^A>A vA^A<vA>^A <A>A<<vA>A>^AAAvA<^A>A
   379A: <v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A
 */

