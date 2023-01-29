using BenchmarkDotNet.Attributes;
using MoreLinq;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    static readonly Dictionary<char, string> Map = new()
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" },
    };

    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var binary = ToBinaryString(input);
        return Packet
            .Parse(binary)
            .Flatten(_ => _.SubPackets)
            .Sum(_ => _.Version);
    }

    static string ToBinaryString(string str) =>
        string.Join(string.Empty, str.Select(_ => Map[_]));
}

class Packet
{
    PacketType type;

    long value;

    private Packet() { }

    public int Version { get; init; }

    public List<Packet> SubPackets { get; init; } = new();

    public static IEnumerable<Packet> Parse(string binary) =>
        InternalParse(binary).Packets;

    static (IEnumerable<Packet> Packets, string rest) InternalParse(
        string binary,
        int subPacketCount = 0,
        int currentSubPacket = 0)
    {
        var packets = new List<Packet>();
        while ((binary != null && binary.Any(_ => _ != '0') && (subPacketCount == 0 || currentSubPacket < subPacketCount)))
        {
            var version = ParseVersion(binary);
            var type = ParseType(binary);

            if (type == PacketType.Literal)
            {
                var (value, rest) = ParseLiteral(binary[6..]);
                packets.Add(new Packet
                {
                    Version = version,
                    type = type,
                    value = value
                });

                if (subPacketCount > 0)
                    currentSubPacket++;

                binary = rest;
            }
            else
            {
                var lengthType = ParseLengthType(binary);
                if (lengthType == LengthType.x15)
                {
                    var packet = new Packet
                    {
                        Version = version,
                        type = type
                    };

                    var (value, rest) = Parse15Bit(binary[7..]);
                    foreach (var p in Parse(value))
                    {
                        packet.SubPackets.Add(p);
                    }

                    packets.Add(packet);

                    if (subPacketCount > 0)
                        currentSubPacket++;

                    binary = rest;
                }
                else
                {
                    var packet = new Packet
                    {
                        Version = version,
                        type = type
                    };

                    var (values, rest) = Parse11Bit(binary[7..]);
                    foreach (var p in values)
                    {
                        packet.SubPackets.Add(p);
                    }

                    packets.Add(packet);

                    if (subPacketCount > 0)
                        currentSubPacket++;

                    binary = rest;
                }
            }
        }

        return (packets, binary ?? string.Empty);
    }

    static int ParseVersion(string packet) =>
        Convert.ToInt32(packet[..3], 2);

    static PacketType ParseType(string packet) =>
        (PacketType)Convert.ToInt32(packet[3..6], 2);

    static LengthType ParseLengthType(string packet) =>
        (LengthType)char.GetNumericValue(packet[6]);

    static (long value, string rest) ParseLiteral(string packet)
    {
        var subPackets = packet
            .Batch(5)
            .Select(_ => _.CreateString())
            .TakeUntil(_ => _.StartsWith("0"))
            .ToList();

        var binary = string.Join(string.Empty, subPackets.Select(_ => _[1..]));
        var value = Convert.ToInt64(binary, 2);
        var rest = packet[subPackets.SelectMany().Count()..];
        return (value, rest);
    }

    static (string value, string rest) Parse15Bit(string packet)
    {
        var subPacketLength = Convert.ToInt32(packet[..15], 2);
        var subPackets = packet.Substring(15, subPacketLength);
        return (subPackets, packet[(15 + subPacketLength)..]);
    }

    static (IEnumerable<Packet> values, string rest) Parse11Bit(string packet)
    {
        var subPacketCount = Convert.ToInt32(packet[..11], 2);
        var subPacketString = packet[11..];
        var (subPackets, rest) = InternalParse(subPacketString, subPacketCount);
        return (subPackets, rest);
    }

    enum PacketType
    {
        Sum,
        Product,
        Minimum,
        Maximum,
        Literal,
        GreaterThan,
        LessThan,
        EqualTo
    }

    enum LengthType
    {
        x15,
        x11
    }
}