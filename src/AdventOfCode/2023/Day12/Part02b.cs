using AocLib;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day12;

public class Part02b : PuzzleSolver<long>
{
    ConcurrentDictionary<string, long> hash = [];
    ConcurrentDictionary<string, Regex> regex = [];

    public override long Solve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _.Split(' ') switch
            {
                var x =>
                (
                    s: x[0],
                    g: x[1].Split(',').Select(int.Parse).ToList()
                )
            })
            .Select(_ => (
                springs: $"{_.s}?{_.s}?{_.s}?{_.s}?{_.s}",
                groups: new List<int>([.._.g, .._.g, .. _.g, .. _.g, .. _.g])
            )).ToList();

        long total = 0, count = 0;
        Parallel.ForEach(lines, line =>
        {
            var permutations = GetPermutations(line);
            Console.WriteLine($"{++count} - {line.springs} [{string.Join(",", line.groups)}] == {permutations}");
            total += permutations;
        });

        return total;
    }

    long GetPermutations((string springs, List<int> groups) line)
    {
        var (springs, groups) = line;

        var key = CreateKey(springs, groups);
        if (hash.ContainsKey(key))
            return hash[key];

        if (springs.Length == 0)
        {
            hash[key] = 0;
            return 0;
        }

        if (groups.Count == 0)
        {
            hash[key] = 1;
            return 1;
        }

        if (springs.Length == groups.Sum() + groups.Count - 1)
        {
            hash[key] = 1;
            return 1;
        }

        if (!IsMatch(springs, groups))
        {
            hash[key] = 0;
            return 0;
        }

        if (springs[0] == '.')
        {
            hash[key] = GetPermutations((springs[1..], groups));
            return hash[key];
        }

        if (springs[0] == '?')
        {
            hash[key] = GetPermutations(($"#{springs[1..]}", groups)) +
                GetPermutations(($".{springs[1..]}", groups));
            return hash[key];
        }

        var end = Math.Min(groups[0] + 1, springs.Length - 1);
        springs = springs[end..];
        groups = groups[1..];

        hash[key] = 0;
        hash[key] += GetPermutations((springs, groups));
        return hash[key];
    }

    string CreateKey(string springs, List<int> groups)
    {
        return $"{springs} [{string.Join(",", groups)}]";
    }

    bool IsMatch(string springs, List<int> groups)
    {
        var key = string.Join(",", groups);
        if (!regex.TryGetValue(key, out var rx))
        {
            var pattern = $"^[.?]{{0,}}{(string.Join("[.?]+?", groups.Select(g => $"([?#]{{{g}}})")))}[.?]*$";
            regex[key] = new Regex(pattern, RegexOptions.Compiled);
        }

        return regex[key].IsMatch(springs);
    }
}


