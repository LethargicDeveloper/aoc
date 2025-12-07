using System.Text.RegularExpressions;
using AocLib;
using Spectre.Console;

namespace _2015.Day07;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var wires = new Dictionary<string, int>();

        var cmds = input
            .SplitLines()
            .Select(x => x.S(" -> "))
            .Select(x => (Op: x[0], Wire: x[1]))
            .ToList();

        var queue = new Queue<(string Op, string Wire)>();
        queue.EnqueueRange(cmds);

        while (queue.TryDequeue(out var cmd))
        {
            var (op, wire) = cmd;

            if (new[] { "AND", "OR", "SHIFT" }.Any(op.Contains))
            {
                var (a, c, b) = Regex.Match(op, @"(\d+) (.*?) (\d+)").GetMatches();
                if (a is null || !wires.ContainsKey(a) || b is null || wires.ContainsKey(b))
                {
                    queue.Enqueue(cmd);
                    continue;
                }

                wires[wire] = c switch
                {
                    "AND" => wires[a] & wires[b],
                    "OR" => wires[a] | wires[b],
                    "LSHIFT" => wires[a] << wires[b],
                    "RSHIFT" => wires[a] >> wires[b],
                    _ => throw new Exception()
                };
            }
            else if (op.Contains("NOT"))
            {
                var a = op.Split("NOT ")[1];
                if (!wires.ContainsKey(a))
                {
                    queue.Enqueue(cmd);
                    continue;
                }
                
                wires[wire] = ~wires[a];
            }
            else
            {
                if (int.TryParse(op, out var value))
                {
                    wires[wire] = value;
                }
                else
                {
                    if (!wires.ContainsKey(op))
                    {
                        queue.Enqueue(cmd);
                        continue;
                    }
                    
                    wires[wire] = wires[op];
                }
            }
        }
        
        return wires["a"];
    }
}
