using System.Text;

namespace AoCSharpLib.AocTypes;

public sealed partial class AocString
{
    public static AocString MatchDigits(AocString str)
        => Join("")(AocRegex.MatchDigits(str));

    public static Func<IEnumerable<AocString>, AocString> Join(AocString separator)
        => (IEnumerable<AocString> values) =>
        {
            var sb = new StringBuilder();
            foreach (var value in values)
            {
                sb.Append(value);
                sb.Append(separator);
            }

            var str = sb.ToString();
            return str.AsSpan()[..(str.Length - separator.Length)].ToString();
        };
}
