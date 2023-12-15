using System.Collections;
using System.Numerics;

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

    public static Point ToPointFromIndex<T>(this T[][] list, int index)
    {
        int x = index % list[0].Length;
        int y = index / list[0].Length;
        return (x, y);
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

    public static long ComputeHash(this char[][] arr)
    {
        long hash = 389;

        for (int y = 0; y < arr.Length; y++)
            for (int x = 0; x < arr[0].Length; x++)
                hash += HashCode.Combine(hash, arr[y][x]);

        return hash;
    }

    public static string ReplaceCharAt(this string input, int index, char newChar)
    {
        ArgumentNullException.ThrowIfNull(input);

        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    public static T[] Rot<T>(this T[] arr, int rot)
    {
        var start = arr[^rot..];
        var end = arr[..^rot];
        return [..start, ..end];
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
