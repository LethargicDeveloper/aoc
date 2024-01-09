using AocLib;
using Microsoft.CodeAnalysis;
using MoreLinq;

namespace AdventOfCode._2023.Day20;

// < 812721756
public class Part02 : PuzzleSolver<long>
{
    class ModuleCollection(List<Module> modules) : IEquatable<ModuleCollection?>
    {
        public IReadOnlyList<Module> Modules => modules;

        public override bool Equals(object? obj)
        {
            return Equals(obj as ModuleCollection);
        }

        public bool Equals(ModuleCollection? other)
        {
            return other is not null &&
                   Modules.Count == other?.Modules.Count &&
                   !Modules.Except(other.Modules).Any();
        }

        public override int GetHashCode()
        {
            var hash = Modules.Aggregate(0, (acc, cur) => HashCode.Combine(acc, cur));
            return hash;
        }

        public static bool operator ==(ModuleCollection? left, ModuleCollection? right)
        {
            return EqualityComparer<ModuleCollection>.Default.Equals(left, right);
        }

        public static bool operator !=(ModuleCollection? left, ModuleCollection? right)
        {
            return !(left == right);
        }
    }

    class Module(string name, string type, List<string> destinations) : IEquatable<Module?>
    {
        public string Name { get; init; } = name;
        public string Type { get; init; } = type;
        public List<string> Destinations { get; init; } = destinations;

        public Dictionary<string, int> LastPulse = [];
        bool on = false;

        public Module(Module module) : this(module.Name, module.Type, module.Destinations.ToList())
        {
            on = module.on;
            LastPulse = LastPulse.ToDictionary(k => k.Key, v => v.Value);
        }

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as Module);
        }

        public bool Equals(Module? other)
        {
            var dict = LastPulse.Count == other?.LastPulse.Count && !LastPulse.Except(other.LastPulse).Any();
            return other is not null &&
                   Name == other.Name &&
                   Type == other.Type &&
                   dict &&
                   on == other.on;
        }

        public override int GetHashCode()
        {
            var hash = LastPulse.Aggregate(0, (acc, cur) => HashCode.Combine(HashCode.Combine(cur.Key, cur.Value), acc));
            return HashCode.Combine(Name, Type, hash, on);
        }

        public static bool operator ==(Module? left, Module? right)
        {
            return EqualityComparer<Module>.Default.Equals(left, right);
        }

        public static bool operator !=(Module? left, Module? right)
        {
            return !(left == right);
        }
    }

    public override long Solve()
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

        var state = new HashSet<ModuleCollection>();

        var broadcaster = modules["broadcaster"];
        var pulseCounts = new int[1];
        pulseCounts = [0, 0];

        long buttonPresses = 0;
        
        Console.Clear();
        for(; ; buttonPresses++)
        {
            pulseCounts[0] = (pulseCounts[0] % 2) + (int)(buttonPresses % int.MaxValue);
            pulseCounts[1] = (pulseCounts[1] % 2) + (int)(buttonPresses % int.MaxValue);
            pulseCounts[0]++;

            var queue = new Queue<(string Origin, string Path, int Pulse, Module Module)>();
            foreach (var dest in broadcaster.Destinations)
            {
                queue.Enqueue(("button", $"button -low-> broadcaster -low-> {dest}", 0, modules[dest]));
            }

            var lastPath = "";
            while (queue.TryDequeue(out var broadcast))
            {
                var (origin, path, pulse, module) = broadcast;
                lastPath = path;
                pulseCounts[pulse]++;

                if (module.Name == "rx")
                {
                    if (pulse == 0)
                        goto END;
                }

                if (module.TryConvertPulse(origin, pulse, out int newPulse))
                {
                    foreach (var newDest in module.Destinations)
                    {
                        var newPath = $"{path} -{(newPulse == 0 ? "low" : "high")}-> {newDest}";
                        if (state.Add(new ModuleCollection(modules.Values.ToList())))
                        {
                            if (modules.TryGetValue(newDest, out Module? m))
                                queue.Enqueue((module.Name, newPath, newPulse, m));
                            else
                                queue.Enqueue((module.Name, newPath, newPulse, new Module(newDest, "", [])));
                        }
                        else
                        {
                            Console.WriteLine("hit");
                        }
                    }
                }
            }


            Console.WriteLine($"{lastPath,-24}");
        }

        END:

        return buttonPresses;
    }
}
