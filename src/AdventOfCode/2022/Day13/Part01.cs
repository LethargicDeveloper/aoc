using AocLib;
using System.Diagnostics;

namespace AdventOfCode._2022.Day13;

public partial class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        return this.input
            .SplitEmptyLines()
            .Select(_ => _.SplitLines().ToList())
            .Select(Packet.Parse)
                .Select((p, i) => (i: i + 1, v: Packet.Compare(p)))
                .Where(_ => _.v <= -1)
                .Sum(_ => _.i);
    }

    class Packet : IComparer<Packet>
    {
        readonly string input;
        readonly List<object> packets;

        public Packet()
            : this("", new List<object>())
        { }

        [DebuggerStepThrough]
        private Packet(string input, List<object> packets)
        {
            this.input = input;
            this.packets = packets;
        }

        public string Input => this.input;

        public static (Packet, Packet) Parse(List<string> packet)
            => packet.Select(CreatePacket).ToArray() switch { var a => (a[0], a[1]) };

        static Packet CreatePacket(string input)
        {
            var packet = new List<object>();
            var stack = new Stack<List<object>>();
            stack.Push(packet);

            var current = packet;
            var doublePop = false;
            for (int i = 1; i < input.Length; ++i)
            {
                var token = input[i];
                if (token == ',') continue;

                if (token == '[')
                {
                    doublePop = true;
                    var list = new List<object>();
                    current.Add(list);
                    stack.Push(list);
                    current = list;
                }
                else if (token == ']')
                {
                    if (doublePop)
                    {
                        stack.TryPop(out _);
                        doublePop = false;
                    }

                    if (!stack.TryPop(out current))
                        break;
                }
                else
                {
                    var index = input.IndexOfAny(new[] { ',', ']' }, i);
                    var value = input[i..index];
                    current.Add(int.Parse(value));
                    i = index - 1;
                }
            }

            return new Packet(input, packet);
        }

        public int Compare(Packet? x, Packet? y)
            => Compare((x ?? new Packet(), y ?? new Packet()));

        public static int Compare((Packet, Packet) packets)
            => CompareInternal(packets).Value;

        static (int Value, bool ExitEarly) CompareInternal((Packet, Packet) packets)
        {
            var (p1, p2) = packets;

            for (int i = 0; i < p1.packets.Count; ++i)
            {
                if (i > p2.packets.Count - 1)
                    return (1, true);

                var p1item = p1.packets[i];
                var p2item = p2.packets[i];

                if (p1item is int int1 && p2item is int int2)
                {
                    if (int1 < int2) return (-1, true);
                    if (int1 > int2) return (1, true);
                }
                else if (p1item is List<object> list1 && p2item is List<object> list2)
                {
                    var result = CompareInternal((new Packet("", list1), new Packet("", list2)));
                    if (result.ExitEarly) return result;
                }
                else
                {
                    var v1 = p1item is List<object> ? (List<object>)p1item : new List<object> { p1item };
                    var v2 = p2item is List<object> ? (List<object>)p2item : new List<object> { p2item };

                    p1.packets[i] = v1;
                    p2.packets[i] = v2;
                    i--;
                }
            }

            var exitEarly = p1.packets.Count < p2.packets.Count;
            var equal = p1.packets.Count == p2.packets.Count ? 0 : -1;
            return (equal, exitEarly);
        }
    }
}
