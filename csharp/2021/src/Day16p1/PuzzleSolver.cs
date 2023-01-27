using BenchmarkDotNet.Attributes;
using QuikGraph;
using QuikGraph.Algorithms;
using System.Diagnostics;
using System.Text.RegularExpressions;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;
    readonly DelegateVertexAndEdgeListGraph<Valve, EquatableEdge<Valve>> graph;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
        this.graph = CreateGraph(ParseInput());
    }

    [Benchmark]
    public long Solve()
    {
        var pairs = (
            from n1 in graph.Vertices.Where(_ => StartNode(_) || ImportantNode(_))
            from n2 in graph.Vertices.Where(_ => StartNode(_) || ImportantNode(_))
            where n1 != n2
            where n2.Name != "AA"
            select (start: n1, end: n2)
        ).Select(_ => ShortestPath(_.start)(_.end).ToList()).ToList();

        return GetBestPressure(pairs);
    }

    static bool StartNode(Valve valve) => valve.Name == "AA";
    static bool ImportantNode(Valve valve) => valve.Rate > 0;

    Func<Valve, IEnumerable<Valve>> ShortestPath(Valve start)
    {
        var previous = new Dictionary<Valve, Valve>();

        var queue = new Queue<Valve>();
        queue.Enqueue(start);

        while (queue.TryDequeue(out var valve))
        {
            foreach (var edge in graph.OutEdges(valve))
            {
                var tunnel = edge.Target;
                if (previous.ContainsKey(tunnel))
                    continue;

                previous[tunnel] = valve;
                queue.Enqueue(tunnel);
            }
        }

        IEnumerable<Valve> shortestPath(Valve end)
        {
            var path = new List<Valve>();

            var current = end;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }

        return shortestPath;
    }

    static long GetBestPressure(List<List<Valve>> pairs)
    {
        var graph = new AdjacencyGraph<Valve, Edge<Valve>>();
        var costs = new Dictionary<Edge<Valve>, double>();

        foreach (var pair in pairs)
        {
            var source = pair[0];
            var target = pair[^1];
            double cost = pair.Count;

            var edge = new Edge<Valve>(source, target);
            graph.AddVerticesAndEdge(edge);
            costs.Add(edge, cost);
        }

        var func = AlgorithmExtensions.GetIndexer(costs);

        var startState = new State
        {
            Valve = graph.Vertices.First(_ => _.Name == "AA"),
            Pressure = 0,
            TimeRemaining = 26
        };

        var queue = new Queue<State>();
        queue.Enqueue(startState);

        long pressure = 0;
        long count = 0;
        while (queue.TryDequeue(out var state))
        {
            pressure = Math.Max(state.Pressure, pressure);

            foreach (var neighbor in graph.OutEdges(state.Valve))
            {
                if (state.Visited.Contains(neighbor.Target))
                    continue;

                var timeRemaning = state.TimeRemaining - (int)func(neighbor);
                if (timeRemaning < 0)
                    continue;

                queue.Enqueue(new State
                {
                    Valve = neighbor.Target,
                    Pressure = state.Pressure + (timeRemaning * neighbor.Target.Rate),
                    TimeRemaining = timeRemaning,
                    Visited = state.Visited.Concat(new[] { state.Valve }).ToHashSet()
                });

                count = Math.Max(queue.Count, count);
            }
        }

        return pressure;
    }

    static DelegateVertexAndEdgeListGraph<Valve, EquatableEdge<Valve>> CreateGraph(Dictionary<Valve, List<Valve>> dict) =>
        dict.ToVertexAndEdgeListGraph(
            kv => Array.ConvertAll(
                kv.Value.ToArray(),
                v => new EquatableEdge<Valve>(kv.Key, v)));

    Dictionary<Valve, List<Valve>> ParseInput()
    {
        var dict = new Dictionary<Valve, List<Valve>>();
        var keys = new Dictionary<string, Valve>();

        foreach (var line in this.input.SplitLines())
        {
            var valveMatch = ValveRegex().Match(line);
            var name = valveMatch.Groups["valve"].Value;
            var rate = int.Parse(valveMatch.Groups["rate"].Value);
            var tunnelNames = valveMatch.Groups["tunnel"].Captures.Select(_ => _.Value).ToList();

            if (!keys.TryGetValue(name, out var key))
            {
                key = new Valve { Name = name };

                keys[name] = key;
                dict[key] = new List<Valve>();
            }

            key.Rate = rate;

            var tunnels = dict[key];
            foreach (var tunnelName in tunnelNames)
            {
                if (!keys.TryGetValue(tunnelName, out var tunnel))
                {
                    tunnel = new Valve { Name = tunnelName };
                    keys[tunnelName] = tunnel;
                    dict[tunnel] = new List<Valve>();
                }

                tunnels.Add(tunnel);
                key.Tunnels.Add(tunnel);
            }
        }

        return dict;
    }

    [GeneratedRegex("Valve (?<valve>..) .*? rate=(?<rate>\\d+); .*? valves? ((?<tunnel>[A-Z]{2})(?:, )?)+")]
    private static partial Regex ValveRegex();
}

[DebuggerDisplay("{Name}")]
class Valve : IEquatable<Valve?>
{
    public string Name { get; set; } = "";
    public int Rate { get; set; }
    public List<Valve> Tunnels { get; set; } = new();

    public override bool Equals(object? obj)
    {
        return Equals(obj as Valve);
    }

    public bool Equals(Valve? other)
    {
        return other is not null && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public static bool operator ==(Valve? left, Valve? right)
    {
        return EqualityComparer<Valve>.Default.Equals(left, right);
    }

    public static bool operator !=(Valve? left, Valve? right)
    {
        return !(left == right);
    }
}

class State : IEquatable<State?>
{
    public State() { }

    public State(Valve valve, int timeRemaining, long pressure, HashSet<Valve> visited)
    {
        this.Valve = valve;
        this.TimeRemaining = timeRemaining;
        this.Pressure = pressure;
        this.Visited = visited;
    }

    public Valve Valve { get; init; } = new();
    public int TimeRemaining { get; init; }
    public long Pressure { get; init; }
    public HashSet<Valve> Visited { get; init; } = new();

    public override bool Equals(object? obj)
    {
        return Equals(obj as State);
    }

    public bool Equals(State? other)
    {
        return other is not null &&
               EqualityComparer<Valve>.Default.Equals(Valve, other.Valve) &&
               TimeRemaining == other.TimeRemaining &&
               Pressure == other.Pressure &&
               Visited.SetEquals(other.Visited);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Valve);
        hash.Add(TimeRemaining);
        hash.Add(Pressure);
        foreach (var value in Visited.OrderBy(_ => _.Name))
        {
            hash.Add(value);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(State? left, State? right)
    {
        return EqualityComparer<State>.Default.Equals(left, right);
    }

    public static bool operator !=(State? left, State? right)
    {
        return !(left == right);
    }
}