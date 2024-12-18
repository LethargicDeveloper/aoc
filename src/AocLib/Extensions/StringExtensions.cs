using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AocLib;

public static partial class StringExtensions
{
    public static string[] S(this string str, char separator) =>
        str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitLines(this string str) =>
        str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string Expand(this string str, int count) =>
        string.Join(string.Empty, str.Repeat(count));
    
    public static IEnumerable<string> Repeat(this string str, int count)
    {
        for (int i = 0; i < count; i++)
            yield return str;
    }

    public static List<List<T>> ParseNumbers<T>(this string str)
        where T : INumber<T>
    {
        return str
            .SplitLines()
            .Select(line => NumberRegex().Matches(line))
            .Select(nums => nums.Select(n => T.Parse(n.ValueSpan, null)).ToList())
            .ToList();
    }
    
    public static List<List<Point<T>>> ParsePoints<T>(this string str)
        where T : INumber<T>
    {
        return str
            .SplitLines()
            .Select(line => PointRegex().Matches(line))
            .Select(points => points.Select(p => Point<T>.Parse(p.ValueSpan, null)).ToList())
            .ToList();
    }

    [GeneratedRegex(@"[+-]?\d+")]
    private static partial Regex NumberRegex();
    
    [GeneratedRegex(@"\d+,\d+")]
    private static partial Regex PointRegex();
}