using System.Numerics;

namespace AocLib;

public static class MathEx
{
    public static long NumberOfDigits(this long number)
    {
        if (number == 0) 
            return 1; 
        
        return (long)Math.Floor(Math.Log10(Math.Abs(number)) + 1);
    }

    public static (long part1, long part2) Split(this long number)
    {
        var mid = number.NumberOfDigits() / 2;
        var divisor = (long)Math.Pow(10, mid);
        var part1 = number / divisor;
        var part2 = number % divisor;
        return (part1, part2);
    }
    
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

    public static (long, long, long) ExtendedGCF(long a, long b)
    {
        if (a == 0) return (b, 0, 1);
        var (gcd, x, y) = ExtendedGCF(b % a, a);
        return (gcd, y - (b / a) * x, x);
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

    public static bool Approximate(float n1, float n2, float epsilon = 0.001f)
        => MathF.Abs(n1 - n2) < epsilon;
    
    public static bool Approximate(double n1, double n2, double epsilon = 0.001d)
        => Math.Abs(n1 - n2) < epsilon;
    
    public static bool Approximate(decimal n1, decimal n2, decimal epsilon = 0.001m)
        => Math.Abs(n1 - n2) < epsilon;
}

public static class NumericExtensions
{
    public static long LCM(this IEnumerable<long> longs)
        => longs.Aggregate(MathEx.LCM);
    
    public static long GCF(this IEnumerable<long> longs)
        => longs.Aggregate(MathEx.GCF);
}
