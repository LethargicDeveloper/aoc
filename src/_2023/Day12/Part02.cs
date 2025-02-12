using AocLib;
using MoreLinq;
using System.Text.RegularExpressions;

namespace _2023.Day12;

// 1088006519007
public class Part02 : PuzzleSolver<long>
{
    readonly Dictionary<string, long> hash = [];

    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _
                .Split(' ')
                .Pipe(x => (s: x[0], g: x[1].Split(',').Select(int.Parse).ToList())))
            .Select(_ => (
                springs: string.Join("?", _.s.Repeat(5)),
                groups: _.g.Repeat(5).ToList()
            )).ToList();

        long total = 0;
        foreach (var line in lines)
        {
            total += GetCombinations(line);
        }

        return total;
    }

    long GetCombinations((string springs, List<int> groups) line)
    {
        var (springs, groups) = line;
        var key = $"{springs} [{string.Join(",", groups)}]";

        if (hash.TryGetValue(key, out var combos))
            return combos;

        if (springs.Length == 0)
            return 0;

        if (springs[0] == '.')
        {
            hash[key] = GetCombinations((springs[1..], groups));
            return hash[key];
        }

        if (springs[0] == '?')
        {
            hash[key] = GetCombinations(($"#{springs[1..]}", groups));
            hash[key] += GetCombinations(($".{springs[1..]}", groups));
            return hash[key];
        }

        if (springs.Length == groups[0] && groups.Count == 1)
            return springs[..groups[0]].Contains('.') ? 0 : 1;

        if (springs.Length < groups.Sum() + groups.Count - 1)
            return 0;

        if (springs[..groups[0]].Contains('.'))
            return 0;

        if (springs[groups[0]] == '#')
            return 0;

        var newSprings = springs[(groups[0] + 1)..].ToString();
        var newGroups = groups.Skip(1).ToList();

        if (newGroups.Count == 0)
            return newSprings.Contains('#') ? 0 : 1;

        hash[key] = GetCombinations((newSprings, newGroups));
        return hash[key];
    }
}


