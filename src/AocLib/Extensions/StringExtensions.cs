using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AocLib;

public static partial class StringExtensions
{
    extension(string str)
    {
        public string[] S(char separator) =>
            str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        public string[] S(string separator) =>
            str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        public string[] S(params List<int> indexes)
        {
            var strings = new List<string>();

            var prev = 0;
            foreach (var cur in indexes)
            {
                if (cur - 1 < prev) continue;
                var split = str[prev..(cur - 1)];
                if (!string.IsNullOrEmpty(split))
                    strings.Add(split);
                
                prev = cur;
            }
            
            if (indexes[^1] < str.Length)
                strings.Add(str[prev..]);
            
            return strings.ToArray();
        }

        public string[] SplitLines() =>
            str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        public string[] SplitEmptyLines() =>
            str.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

        public T SplitEmptyLines<T>(Func<string[], T> func) =>
            func(SplitEmptyLines(str));

        public string Expand(int count) =>
            string.Join(string.Empty, str.Repeat(count));

        public IEnumerable<string> Repeat(int count)
        {
            for (int i = 0; i < count; i++)
                yield return str;
        }

        public List<List<T>> ParseNumbers<T>()
            where T : INumber<T>
        {
            return str
                .SplitLines()
                .Select(line => NumberRegex().Matches(line))
                .Select(nums => nums.Select(n => T.Parse(n.ValueSpan, null)).ToList())
                .ToList();
        }

        public List<List<Point<T>>> ParsePoints<T>()
            where T : INumber<T>
        {
            return str
                .SplitLines()
                .Select(line => PointRegex().Matches(line))
                .Select(points => points.Select(p => Point<T>.Parse(p.ValueSpan, null)).ToList())
                .ToList();
        }
    }

    [GeneratedRegex(@"[+-]?\d+")]
    private static partial Regex NumberRegex();
    
    [GeneratedRegex(@"\d+,\d+")]
    private static partial Regex PointRegex();
}