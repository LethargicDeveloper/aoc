using System.Text;

namespace AocLib;

public static class Extensions
{
    public static Point FindPosition<T>(this T[][] list, T value)
    {
        for (int y = 0; y < list.Length; y++)
        for (int x = 0; x < list[0].Length; x++)
        {
            if (list[y][x]?.Equals(value) ?? false)
                return new Point(x, y);
        }

        throw new KeyNotFoundException();
    }
    
    public static T At<T>(this T[][] grid, Point pos) => grid[pos.Y][pos.X];

    public static void Print(this char[][] list, bool clear = true, bool pause = false, List<Point>? overlay = null)
    {
        var over = overlay ?? [];
        
        if (clear) Console.Clear();
        
        var sb = new StringBuilder();

        for (int y = 0; y < list.Length; y++)
        {
            for (int x = 0; x < list[0].Length; x++)
            {
                if (over.Contains((x, y)))
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('@');
                    Console.ForegroundColor = color;
                }
                else
                {
                    Console.Write(list[y][x]);
                }
            }
            
            Console.WriteLine();
        }
        
        Console.Write(sb);

        if (pause) Console.ReadLine();
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
    
    public static long ComputeHash<T>(this List<T> arr)
    {
        var hashCode = new HashCode();
        foreach (var item in arr)
            hashCode.Add(item);
        
        return hashCode.ToHashCode();
    }

    public static long ComputeHash<T>(this T[] arr)
    {
        var hashCode = new HashCode();
        foreach (var item in arr)
            hashCode.Add(item);
        
        return hashCode.ToHashCode();
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

    public static void AddRange<T>(this HashSet<T> hash, IEnumerable<T> range)
    {
        foreach (var r in range)
            _ = hash.Add(r);
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
