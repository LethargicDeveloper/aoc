using System.Numerics;

namespace AocLib;

public static class MathEx
{
    public static T Lcm<T>(T a, T b)
        where T : INumber<T> => (a / Gcd(a, b)) * b;
    
    public static T Gcd<T>(T a, T b)
        where T : INumber<T>
    {
        while (b != T.Zero)
        {
            T temp = b;
            b = a % b;
            a = temp;
        }
    
        return a;
    }
    
    public static int DigitCount<T>(this T number)
        where T : INumber<T>
    {
        if (number == T.Zero) 
            return 1;

        double value = Convert.ToDouble(number);
        return (int)Math.Floor(Math.Log10(value) + 1);
    }
    
    public static bool ApproximateEquals<T>(T n1, T n2, T? tolerance = default)
        where T : IBinaryFloatingPointIeee754<T>
        => T.Abs(n1 - n2) < (tolerance == default ? T.CreateTruncating(0.001) : tolerance!);
    
    public static T Mod<T>(T num, T mod)
        where T : INumber<T>
    {
        var r = num % mod;
        return r < T.Zero ? r + mod : r;
    }

    // public static T Max<T>(params T[] values)
    //     where T : struct, INumber<T>
    //     => (values ?? []).Aggregate((acc, cur) => acc > cur ? acc : cur);
    //
    // public static T Min<T>(params T[] values)
    //     where T : struct, INumber<T>
    //     => (values ?? []).Aggregate((acc, cur) => acc < cur ? acc : cur);
    //
    // public static (long, long, long) ExtendedGCF(long a, long b)
    // {
    //     if (a == 0) return (b, 0, 1);
    //     var (gcd, x, y) = ExtendedGCF(b % a, a);
    //     return (gcd, y - (b / a) * x, x);
    // }
    //
    // public static T Factorial<T>(T num)
    //     where T : struct, INumber<T>
    // {
    //     if (num < T.One) return T.One;
    //
    //     if (num == T.MultiplicativeIdentity)
    //         return T.MultiplicativeIdentity;
    //
    //     return num * Factorial(num - T.One);
    // }
    //
    // public static T Combinations<T>(T num1, T num2)
    //     where T : struct, INumber<T>
    //     => Factorial(num1) / (Factorial(num2) * Factorial(num1 - num2));
    //
    // public static T TriangularNumber<T>(T n)
    //     where T : struct, INumber<T>
    //     => ((n * n) + n) / (T.One + T.One);
}

public static class NumericExtensions
{
    public static T Lcm<T>(this IEnumerable<T> list)
        where T : INumber<T> =>
        list.Aggregate(MathEx.Lcm);
    
    public static T Gcd<T>(this IEnumerable<T> list)
        where T : INumber<T> =>
        list.Aggregate(MathEx.Gcd);
    
    public static bool ApproximateEquals<T>(this T n1, T n2, T? tolerance = default)
        where T : IBinaryFloatingPointIeee754<T> =>
        MathEx.ApproximateEquals(n1, n2, tolerance);
    
    public static T Mod<T>(this T num, T mod)
        where T : INumber<T> => MathEx.Mod(num, mod);
}
