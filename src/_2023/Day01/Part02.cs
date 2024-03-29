using AocLib;
using MoreLinq;
using System.Text.RegularExpressions;

namespace _2023.Day01;

// 53866
public class Part02 : PuzzleSolver<long>
{
    static readonly Dictionary<string, string> numbers = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };

    protected override long InternalSolve()
    {
        static string Replace(string val) =>
            numbers.TryGetValue(val, out var result) ? result : val;

        return input
            .SplitLines()
            .Select(_ => Regex.Matches(_, $"(?=({string.Join('|', numbers.Keys.Concat(numbers.Values))}))"))
            .Select(_ => Replace(_.First().Groups[1].Value) + Replace(_.Last().Groups[1].Value))
            .Select(int.Parse)
            .Sum();
    }
}
