namespace AocLib;

public static class StringExtensions
{
    public static string[] SplitLines(this string str)
        => str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}
