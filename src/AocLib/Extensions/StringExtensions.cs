namespace AocLib;

public static class StringExtensions
{
    public static string[] SplitLines(this string str)
        => str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitR(this string str, char separator)
        => str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

    public static IEnumerable<string> Repeat(this string input, int count)
    {
        for (int i = 0; i < count; i++)
            yield return input;
    }
}
