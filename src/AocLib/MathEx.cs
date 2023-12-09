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
        => longs.Aggregate(MathEx.LCM);
}
