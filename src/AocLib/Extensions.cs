using System.Numerics;
using static QuikGraph.Algorithms.Assignment.HungarianAlgorithm;

namespace AocLib;

public static class Extensions
{
    public static void Log(this object obj)
        => Console.WriteLine(obj);

    public static string CreateString(this IEnumerable<char> list)
        => new(list.ToArray());

    public static T Mod<T>(this T num, T mod)
        where T : struct, INumber<T>
    {
        var r = num % mod;
        return r < T.Zero ? r + mod : r;
    }

    public static CustomIntEnumerator GetEnumerator(this Range range)
        => new(range);

    public static IEnumerable<int> Range(this Range range)
    {
        foreach (var i in range)
        {
            yield return i;
        }
    }
}

public struct CustomIntEnumerator
{
    private int current;
    private readonly int end;

    public CustomIntEnumerator(Range range)
    {
        if (range.End.IsFromEnd)
        {
            throw new NotSupportedException();
        }

        current = range.Start.Value - 1;
        end = range.End.Value;
    }

    public int Current => current;

    public bool MoveNext()
    {
        current++;
        return current <= end;
    }
}
