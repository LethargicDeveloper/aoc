namespace AocLib;

public static class Extensions
{
    public static string[] SplitLines(this string str)
        => str.Split(Environment.NewLine);

    public static string[] SplitEmptyLines(this string str)
        => str.Split($"{Environment.NewLine}{Environment.NewLine}");
}
