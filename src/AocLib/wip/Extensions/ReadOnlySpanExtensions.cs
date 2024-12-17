namespace AocLib;

public static class ReadOnlySpanExtensions
{
    public static ReadOnlySpan<char> Mirror(this ReadOnlySpan<char> str)
        => str.ToString().Reverse().AsString().AsSpan();
}