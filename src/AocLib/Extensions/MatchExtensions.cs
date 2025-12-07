using System.Numerics;
using System.Text.RegularExpressions;
using AocLib.Utility;

namespace AocLib;

public static class MatchExtensions
{
    extension(Match match)
    {
        public string Get(string groupName) =>
            match.Groups[groupName].Value;

        public string Get(int groupIndex) =>
            match.Groups[groupIndex].Value;
        
        public string[] GetMatches() =>
            match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();

        public List<T> ParseNumbers<T>()
            where T : struct, INumber<T>
        {
            return match.Groups.Values
                .Select(g => T.TryParse(g.Value.AsSpan(), null, out var result)
                    ? Maybe<T>.Some(result)
                    : Maybe<T>.None())
                .Where(v => v.HasValue)
                .Select(v => v.Value)
                .ToList();
        }
    }
    
    extension(Match match)
    {
        public Dictionary<string, string> ToMatchDictionary() =>
            match.Groups.Values.Skip(1).ToDictionary(k => k.Name, v => v.Value);

        public Dictionary<TKey, TValue> ToMatchDictionary<TKey, TValue>(Func<string, TKey> keySelector, Func<string, TValue> valueSelector)
            where TKey : notnull =>
            match.Groups.Values.Skip(1).ToDictionary(k => keySelector(k.Value), v => valueSelector(v.Value));
    }
    
    public static Dictionary<TKey, TValue> ToMatchDictionary<TKey, TValue>(
        this IEnumerable<Match> matches, Func<string[], TKey> keySelector, Func<string[], TValue> valueSelector)
        where TKey : notnull => matches
        .Select(m => m.Groups.Values.Select(v => v.Value).ToArray())
        .ToDictionary(keySelector, valueSelector);
}
