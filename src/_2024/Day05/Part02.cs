using System.Text.RegularExpressions;
using AocLib;
using MoreLinq;

namespace _2024.Day05;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumbersRegex();

    protected override long InternalSolve()
    {
        var parts = input.SplitEmptyLines();

        var rules = parts[0]
            .SplitLines()
            .Select(x => new[] { int.Parse(x[..2]), int.Parse(x[^2..]) })
            .ToDictionary(k => Hash(k[0], k[1]), v => v[0]);

        var updates = parts[1]
            .SplitLines()
            .Select(x => x.Split(",")
                .Select(int.Parse).ToList())
            .ToList();

        var answer = updates
            .Where(x => !IsOrdered(x, rules))
            .Select(x => Order(x, rules))
            .Select(x => x[x.Count / 2])
            .Sum();

        return answer;
    }

    static bool IsOrdered(List<int> update, Dictionary<int, int> rules)
    {
        for (int i = 1; i < update.Count; i++)
        {
            var num1 = update[i];

            for (int j = i - 1; j >= 0; j--)
            {
                var num2 = update[j];

                var hash = Hash(num1, num2);
                var first = rules[hash];

                if (first == num1)
                    return false;
            }
        }

        return true;
    }

    static List<int> Order(List<int> unorderedList, Dictionary<int, int> rules)
    {
        var list = unorderedList.ToList();
        
        for (int i = 1; i < list.Count; i++)
        {
            var num1 = list[i];

            for (int j = i - 1; j >= 0; j--)
            {
                var num2 = list[j];

                var hash = Hash(num1, num2);
                var first = rules[hash];

                if (first == num1)
                {
                    var index = list.IndexOf(num1);
                    
                    list[index] = num2;
                    list[j] = num1;
                }
            }
        }

        return list;
    }

    static int Hash(int num1, int num2)
    {
        int min = Math.Min(num1, num2);
        int max = Math.Max(num1, num2);

        return (min * 73856093) ^ (max * 19349663);
    }
}