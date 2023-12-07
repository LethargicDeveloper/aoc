using System.Collections;

namespace AoCSharpLib.AocTypes;

public sealed partial class AocString :
    ICloneable,
    IComparable,
    IComparable<AocString>,
    IConvertible,
    IEquatable<AocString>,
    IParsable<AocString>,
    ISpanParsable<AocString>,
    IEnumerable<AocString>
{
    public readonly static AocString Empty = new(string.Empty);

    readonly string str;

    public AocString()
    {
        this.str = string.Empty;
    }

    public AocString(string str)
    {
        this.str = str;
    }

    public AocInt Length => this.str.Length;

    public AocString this[Index i]
    {
        get => str[i].ToString();
    }

    #region ICloneable
    public object Clone() => str.Clone();
    #endregion

    #region IComparable
    public int CompareTo(object? obj) => str.CompareTo(obj);
    #endregion

    #region IComparable<AocString>
    public int CompareTo(AocString? other) => CompareTo(other);
    #endregion

    #region IConvertible
    TypeCode IConvertible.GetTypeCode() => str.GetTypeCode();

    bool IConvertible.ToBoolean(IFormatProvider? provider) => ((IConvertible)str).ToBoolean(provider);

    byte IConvertible.ToByte(IFormatProvider? provider) => ((IConvertible)str).ToByte(provider);

    char IConvertible.ToChar(IFormatProvider? provider) => ((IConvertible)str).ToChar(provider);

    DateTime IConvertible.ToDateTime(IFormatProvider? provider) => ((IConvertible)str).ToDateTime(provider);

    decimal IConvertible.ToDecimal(IFormatProvider? provider) => ((IConvertible)str).ToDecimal(provider);

    double IConvertible.ToDouble(IFormatProvider? provider) => ((IConvertible)str).ToDouble(provider);

    short IConvertible.ToInt16(IFormatProvider? provider) => ((IConvertible)str).ToInt16(provider);

    int IConvertible.ToInt32(IFormatProvider? provider) => ((IConvertible)str).ToInt32(provider);

    long IConvertible.ToInt64(IFormatProvider? provider) => ((IConvertible)str).ToInt64(provider);

    sbyte IConvertible.ToSByte(IFormatProvider? provider) => ((IConvertible)str).ToSByte(provider);

    float IConvertible.ToSingle(IFormatProvider? provider) => ((IConvertible)str).ToSingle(provider);

    string IConvertible.ToString(IFormatProvider? provider) => str.ToString(provider);

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider) => ((IConvertible)str).ToType(conversionType, provider);

    ushort IConvertible.ToUInt16(IFormatProvider? provider) => ((IConvertible)str).ToUInt16(provider);

    uint IConvertible.ToUInt32(IFormatProvider? provider) => ((IConvertible)str).ToUInt32(provider);

    ulong IConvertible.ToUInt64(IFormatProvider? provider) => ((IConvertible)str).ToUInt64(provider);
    #endregion

    #region IEquatable<AocString>
    public bool Equals(AocString? other) => Equals(other);
    #endregion

    #region IParsabler<AocString>
    static AocString IParsable<AocString>.Parse(string s, IFormatProvider? provider)
        => new(s);

    static bool IParsable<AocString>.TryParse(string? s, IFormatProvider? provider, out AocString result)
    {
        if (s == null)
        {
            result = Empty;
            return false;
        }

        result = new(s);
        return true;
    }
    #endregion

    #region ISpanParsable<AocString>
    static AocString ISpanParsable<AocString>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(s.ToString());

    static bool ISpanParsable<AocString>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out AocString result)
    {
        result = new(s.ToString());
        return true;
    }
    #endregion

    #region IEnumerable<AocString>
    public IEnumerator<AocString> GetEnumerator()
    {
        foreach (var chr in str)
            yield return chr.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region Equality
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;

        return Equals((AocString)obj);
    }

    public override int GetHashCode() => str.GetHashCode();

    public static bool operator ==(AocString? left, AocString? right)
        => left?.str == right?.str;

    public static bool operator !=(AocString? left, AocString? right)
        => left?.str != right?.str;

    public static bool operator <(AocString? left, AocString? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(AocString? left, AocString? right)
        => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(AocString? left, AocString? right)
        => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(AocString? left, AocString? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;
    #endregion

    public override string ToString() => str;

    public static implicit operator AocString(string str)
        => new(str);

    public static implicit operator string(AocString aocString)
        => aocString.str;

    public static explicit operator long(AocString aocString)
        => long.TryParse(aocString.str, out var result) ? result : 0;

    public IEnumerable<AocString> _S => str
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Select(_ => new AocString(_));

    public IEnumerable<AocString> _SS => str
        .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
        .Select(_ => new AocString(_));
}
