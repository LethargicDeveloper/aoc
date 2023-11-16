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
}
