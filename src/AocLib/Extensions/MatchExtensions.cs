using System.Numerics;
using System.Text.RegularExpressions;
using AocLib.Utility;

namespace AocLib;

public static class MatchExtensions
{
    public static string Get(this Match match, string groupName) =>
        match.Groups[groupName].Value;
    
    public static string Get(this Match match, int groupIndex) =>
        match.Groups[groupIndex].Value;
    
    public static List<T> ParseNumbers<T>(this Match match)
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

    public static Dictionary<TKey, TValue> ToMatchDictionary<TKey, TValue>(
        this IEnumerable<Match> matches, Func<string[], TKey> keySelector, Func<string[], TValue> valueSelector)
        where TKey : notnull => matches
            .Select(m => m.Groups.Values.Select(v => v.Value).ToArray())
            .ToDictionary(keySelector, valueSelector);
    
    public static Dictionary<string, string> ToMatchDictionary(this Match match) =>
        match.Groups.Values.Skip(1).ToDictionary(k => k.Name, v => v.Value);
    
    public static Dictionary<TKey, TValue> ToMatchDictionary<TKey, TValue>(
        this Match match, Func<string, TKey> keySelector, Func<string, TValue> valueSelector)
        where TKey : notnull =>
        match.Groups.Values.Skip(1).ToDictionary(k => keySelector(k.Value), v => valueSelector(v.Value));
}
