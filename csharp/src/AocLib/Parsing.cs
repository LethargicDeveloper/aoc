namespace AocLib;

public static class Parsing
{
    public static string[] SplitLines(this string str)
        => str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitEmptyLines(this string str)
        => str.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

    public static T SplitEmptyLines<T>(this string str, Func<string[], T> func)
        => func(SplitEmptyLines(str));
}
