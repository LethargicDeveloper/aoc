using System.Numerics;

namespace AocLib;

public static class MathEx
{
    public static T Max<T>(params T[] values)
        where T : struct, INumber<T>
        => (values ?? []).Aggregate((acc, cur) => acc > cur ? acc : cur);

    public static T Min<T>(params T[] values)
        where T : struct, INumber<T>
        => (values ?? []).Aggregate((acc, cur) => acc < cur ? acc : cur);

    public static T LCM<T>(T a, T b)
        where T : struct, INumber<T>
        => (a / GCF(a, b)) * b;

    public static T GCF<T>(T a, T b)
        where T : struct, INumber<T>
    {
        while (b != T.Zero)
        {
            T temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T Mod<T>(T num, T mod)
        where T : struct, INumber<T>
    {
        var r = num % mod;
        return r < T.Zero ? r + mod : r;
    }

    public static T Factorial<T>(T num)
        where T : struct, INumber<T>
    {
        if (num < T.One) return T.One;

        if (num == T.MultiplicativeIdentity)
            return T.MultiplicativeIdentity;

        return num * Factorial(num - T.One);
    }

    public static T Combinations<T>(T num1, T num2)
        where T : struct, INumber<T>
        => Factorial(num1) / (Factorial(num2) * Factorial(num1 - num2));

    public static T TriangularNumber<T>(T n)
        where T : struct, INumber<T>
        => ((n * n) + n) / (T.One + T.One);
}

public static class NumericExtensions
{
    public static long LCM(this IEnumerable<long> longs)
        => longs.Aggregate(MathEx.LCM);
}
