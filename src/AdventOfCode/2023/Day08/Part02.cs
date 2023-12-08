using AdventOfCode.Abstractions;
using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day08;

// 8811050362409
public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var group = input.SplitEmptyLines();
        var cmds = group[0];
        var network = group[1]
            .SplitLines()
            .Select(_ =>
                Regex.Matches(_, "[\\dA-Z]{3}") switch
                {
                    var m => (k: m[0].Value, l: m[1].Value, r: m[2].Value)
                })
            .ToDictionary(k => k.k, _ => new Dictionary<char, string> { { 'L', _.l }, { 'R', _.r } });

        var keys = network.Keys.Where(_ => _.EndsWith('A')).ToList();
        var steps = new long[keys.Count];

        foreach (var cmd in GetCommands(cmds))
        {
            for (int i = 0; i < keys.Count; ++i)
            {
                keys[i] = network[keys[i]][cmd];
                steps[i]++;
            }

            keys = keys.Where(_ => !_.EndsWith('Z')).ToList();

            if (keys.All(_ => _.EndsWith('Z')))
                break;
        }

        return steps.LCM();
    }

    IEnumerable<char> GetCommands(string cmds)
    {
        while (true)
            foreach (var cmd in cmds)
                yield return cmd;
    }
}
