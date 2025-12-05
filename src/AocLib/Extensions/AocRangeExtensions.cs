using System.Numerics;

namespace AocLib;

public static class AocRangeExtensions
{
    public static IEnumerable<AocRange<T>> OrderRanges<T>(this IEnumerable<AocRange<T>> ranges)
        where T : INumber<T>
    {
        return ranges.OrderBy(r => r.Start).ThenBy(r => r.End);
    }
    
    public static IEnumerable<AocRange<T>> MergeRanges<T>(this IEnumerable<AocRange<T>> ranges)
        where T : INumber<T>
    {
        var list = new List<AocRange<T>>();

        foreach (var range in ranges.OrderRanges())
        {
            if (list.Count == 0)
                list.Add(range);
            else
            {
                var newRanges = range.Merge(list[^1]);
                list.RemoveAt(list.Count - 1);
                list.AddRange(newRanges);
            }
        }

        return list;
    }
}