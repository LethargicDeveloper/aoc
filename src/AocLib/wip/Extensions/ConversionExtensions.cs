namespace AocLib;

public static class ConversionExtensions
{
    public static int AsInt(this ReadOnlySpan<char> input)
    {
        int val = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            val += (i * 10) + (input[i] - '0');
        }

        return val;
    }

    public static int AsInt(this string input) => AsInt(input.AsSpan());
}