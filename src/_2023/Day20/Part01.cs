using AocLib;
using Microsoft.CodeAnalysis;

namespace _2023.Day20;

// 812721756
public class Part01 : PuzzleSolver<long>
{
    class Module(string name, string type, List<string> destinations)
    {
        public string Name { get; init; } = name;
        public string Type { get; init; } = type;
        public List<string> Destinations { get; init; } = destinations;

        public Dictionary<string, int> LastPulse = [];
        bool on = false;

        public bool TryConvertPulse(string origin, int pulse, out int newPulse)
        {
            newPulse = 0;
            switch (Type)
            {
                case "%":
                    if (pulse == 1) return false;
                    newPulse = on ? 0 : 1;
                    on = !on;
                    return true;
                case "&":
                    if (!LastPulse.ContainsKey(origin))
                        LastPulse[origin] = 0;

                    LastPulse[origin] = pulse;
                    newPulse = LastPulse.Values.All(_ => _ == 1) ? 0 : 1;
                    return true;
            }

            return false;
        }
    }

    protected override long InternalSolve()
    {
        var modules = input
            .SplitLines()
            .Select(_ => _.Split(" -> ", StringSplitOptions.RemoveEmptyEntries) switch
            {
                var x => (module: x[0], dest: x[1])
            })
            .Select(_ => _.module switch
            {
                "broadcaster" => new Module(_.module, _.module, [.. _.dest.Split(", ")]),
                _ => new Module(_.module[1..], $"{_.module[0]}", [.. _.dest.Split(", ")]),
            })
            .ToDictionary(k => k.Name, v => v);

        foreach (var con in modules.Values.Where(_ => _.Type == "&"))
        {
            con.LastPulse = modules.Values
                .Where(_ => _.Destinations.Contains(con.Name))
                .ToDictionary(k => k.Name, _ => 0);
        }

        var broadcaster = modules["broadcaster"];

        const int pushes = 1000;
        var pulseCounts = new long[pushes][];
        for (int i = 0; i < pulseCounts.Length; i++)
            pulseCounts[i] = [0, 0];

        for (var i = 0; i < pushes; i++)
        {
            pulseCounts[i][0]++;

            var queue = new Queue<(string Origin, string Path, int Pulse, Module Module)>();
            foreach (var dest in broadcaster.Destinations)
            {
                queue.Enqueue(("button", $"broadcaster -low-> {dest}", 0, modules[dest]));
            }

            while (queue.TryDequeue(out var broadcast))
            {
                var (origin, path, pulse, module) = broadcast;
                pulseCounts[i][pulse]++;

                var tlow = pulseCounts.Sum(_ => _[0]);
                var thigh = pulseCounts.Sum(_ => _[1]);

                if (module.TryConvertPulse(origin, pulse, out int newPulse))
                {
                    foreach (var newDest in module.Destinations)
                    {
                        var newPath = $"{module.Name} -{(newPulse == 0 ? "low" : "high")}-> {newDest}";
                        if (modules.TryGetValue(newDest, out Module? m))
                            queue.Enqueue((module.Name, newPath, newPulse, m));
                        else
                            queue.Enqueue((module.Name, newPath, newPulse, new Module(newDest, "", [])));
                    }
                }
            }
        }

        var low = pulseCounts.Sum(_ => _[0]);
        var high = pulseCounts.Sum(_ => _[1]);
        return low * high;
    }
}
