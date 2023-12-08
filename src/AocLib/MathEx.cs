using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AocLib;

public static class MathEx
{
    public static long LCM(long a, long b)
        => (a / GCF(a, b)) * b;

    public static long GCF(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}

public static class NumericExtensions
{
    public static long LCM(this long[] longs)
    {
        long acc = longs[0];
        for (int i = 1; i < longs.Length; ++i)
        {
            acc = MathEx.LCM(acc, longs[i]);
        }

        return acc;
    }
}
