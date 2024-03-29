using AocLib;
using System.Data;

namespace _2023.Day20;

// 233338595643977
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _.Split(" -> ") switch
            {
                var x => (type: x[0][0], name: x[0][1..], connections: x[1].Split(", "))
            });

        var modules = lines
            .Select(_ => _.type switch
            {
                '%' => new Module(_.type, _.name, _.connections),
                '&' => new Module(_.type, _.name, _.connections, lines
                    .Where(x => x.connections.Contains(_.name))
                    .Select(x => x.name)
                    .ToArray()),
                _ => new Module('b', "broadcaster", _.connections)
            })
            .ToDictionary(k => k.Name, v => v);

        string endNode = "rx";
        string endNodeParent = modules.Single(_ => _.Value.Destinations.Contains(endNode)).Key;
        var endNodeParentParents = modules.Where(_ => _.Value.Destinations.Contains(endNodeParent))
            .Select(_ => _.Key)
            .ToList();

        var dict = new Dictionary<string, long>();

        for (long button = 1; ; button++)
        {
            var queue = new Queue<Pulse>();
            queue.Enqueue(new Pulse("button", "broadcaster", 0));

            while (queue.TryDequeue(out var pulse))
            {
                var (source, destination, pulseType) = pulse;

                if (destination == endNodeParent && pulseType == 1)
                {
                    dict.TryAdd(source, button);
                    if (endNodeParentParents.All(dict.ContainsKey))
                        return dict.Values.Product();
                }

                if (!modules.ContainsKey(destination))
                    modules[destination] = new Module('0', destination, []);

                var nextModule = modules[destination];
                if (nextModule.TryConvertPulse(source, pulseType, out int nextPulseType))
                    foreach (var nextDestination in nextModule.Destinations)
                        queue.Enqueue(new Pulse(destination, nextDestination, nextPulseType));
            }
        }
    }

    record Pulse(string Source, string Destination, int PulseType);

    class Module(char type, string name, string[] destinations, string[]? sources = null)
    {
        public string Name { get; init; } = name;
        public char Type { get; init; } = type;
        public string[] Destinations { get; init; } = destinations;
        public string[] Sources { get; init; } = sources ?? [];

        public Dictionary<string, int> LastPulse =
            (sources ?? []).ToDictionary(k => k, v => 0);

        bool on = false;

        public bool TryConvertPulse(string origin, int pulse, out int newPulse)
        {
            newPulse = 0;
            switch (Type)
            {
                case '%':
                    if (pulse == 1) return false;
                    newPulse = (on = !on) ? 1 : 0;
                    return true;
                case '&':
                    if (!LastPulse.ContainsKey(origin))
                        LastPulse[origin] = 0;

                    LastPulse[origin] = pulse;
                    newPulse = LastPulse.Values.All(_ => _ == 1) ? 0 : 1;
                    return true;
                case 'b':
                    newPulse = pulse;
                    return true;
            }

            return false;
        }
    }
}