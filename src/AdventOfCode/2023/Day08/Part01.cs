using AocLib;
using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day08;

// 19637
public class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var group = input.SplitEmptyLines();
        var cmds = group[0];
        var network = group[1]
            .SplitLines()
            .Select(_ =>
                Regex.Matches(_, "[A-Z]{3}") switch
                {
                    var m => (k: m[0].Value, l: m[1].Value, r: m[2].Value)
                })
            .ToDictionary(k => k.k, _ => new Dictionary<char, string> { { 'L', _.l }, { 'R', _.r } });

        var key = "AAA";
        int steps = 0;

        foreach (var cmd in GetCommands(cmds))
        {
            key = network[key][cmd];
            steps++;
            
            if (key == "ZZZ") break;
        }

        return steps;
    }

    IEnumerable<char> GetCommands(string cmds)
    {
        while (true)
            foreach (var cmd in cmds)
                yield return cmd;
    }
}
