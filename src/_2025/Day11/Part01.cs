using System.Text.RegularExpressions;

namespace _2025.Day11;

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var devices = input
            .SplitLines()
            .Select(line =>
            {
                var parts = Regex.Split(line, ":");
                var name = parts[0];
                var outputs = parts[1].S(' ').ToList();
                
                return (name, outputs);
            })
            .ToDictionary(k => k.name, v => v.outputs);
        
        long total = 0;
        
        var stack = new Stack<List<string>>();
        stack.Push(["you"]);
        
        while (stack.TryPop(out var path))
        {
            if (path[^1] == "out")
            {
                total++;
                continue;
            }
        
            foreach (var output in devices[path[^1]])
            {
                if (!path.Contains(output))
                    stack.Push([..path, output]);
            }
        }
        
        return total;
    }

    record Device(string Name, List<string> Outputs);
}
