using System.Text.RegularExpressions;

namespace _2025.Day10;

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Select(line => LineRegex().Match(line).GetMatches())
            .Select(parts =>
            {
                var (lights, buttons) = parts;

                return (
                    Target: lights![1..^1].Select(l => l == '#').ToArray(),
                    Buttons: buttons!.Split(' ').Select(b => b[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray()
                );
            })
            .ToList();

        var total = 0;
        
        foreach (var (target, buttons) in lines)
        {
            var queue = new Queue<(bool[] Lights, int Depth)>();
            foreach (var button in buttons)
                queue.Enqueue((Press(button, new bool[target.Length]), 1));

            while (queue.TryDequeue(out var state))
            {
                var (lights, depth) = state;
                
                if (lights.SequenceEqual(target))
                {
                    total += depth;
                    break;
                }
                
                foreach (var button in buttons)
                    queue.Enqueue((Press(button, [..lights]), depth + 1));
            }
        }

        return total;

        bool[] Press(int[] button, bool[] lights)
        {
            foreach (var num in button)
                lights[num] = !lights[num];

            return lights;
        }
    }

    [GeneratedRegex(@"^(\[.*\])\s(.*?)\s(\{.*?\})$")]
    private static partial Regex LineRegex();
}
