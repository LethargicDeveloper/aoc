using System.Text.RegularExpressions;

namespace AoCSharpLib.AocTypes;

public sealed partial class AocRegex
{
    public static IEnumerable<AocString> MatchDigits(AocString str)
        => RegexMatchDigits().Matches(str).Select(_ => new AocString(_.Value));

    [GeneratedRegex("\\d+")]
    private static partial Regex RegexMatchDigits();
}
