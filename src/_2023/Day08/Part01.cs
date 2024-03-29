using AocLib;
using System.Text.RegularExpressions;

namespace _2023.Day08;

// 19637
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var group = input.SplitEmptyLines();
        var cmds = group[0];
        var network = group[1]
            .SplitLines()
            .Select(_ => Regex
                .Matches(_, "[A-Z]{3}")
                .Pipe(m => (k: m[0].Value, l: m[1].Value, r: m[2].Value)))
            .ToDictionary(k => k.k, _ => new Dictionary<char, string> { { 'L', _.l }, { 'R', _.r } });

        var key = "AAA";
        int steps = 0;

        foreach (var cmd in cmds.Loop())
        {
            key = network[key][cmd];
            steps++;

            if (key == "ZZZ") break;
        }

        return steps;
    }
}
