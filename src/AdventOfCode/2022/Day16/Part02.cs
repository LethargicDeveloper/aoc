using AocLib;
using QuikGraph;
using QuikGraph.Algorithms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day16;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex("Valve (?<valve>..) .*? rate=(?<rate>\\d+); .*? valves? ((?<tunnel>[A-Z]{2})(?:, )?)+")]
    private static partial Regex ValveRegex();

    static DelegateVertexAndEdgeListGraph<Valve, EquatableEdge<Valve>> CreateGraph(Dictionary<Valve, List<Valve>> dict) =>
        dict.ToVertexAndEdgeListGraph(
            kv => Array.ConvertAll(
                kv.Value.ToArray(),
                v => new EquatableEdge<Valve>(kv.Key, v)));

    static bool StartNode(Valve valve) => valve.Name == "AA";

    static bool ImportantNode(Valve valve) => valve.Rate > 0;

    static long GetElephantPressure(List<List<Valve>> pairs)
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

        var initialState = new State
        {
            Valve = graph.Vertices.First(_ => _.Name == "AA"),
            Pressure = 0,
            TimeRemaining = 26
        };

        var hash = new HashSet<(State, State)>();
        var queue = new Queue<(State, State)>();

        hash.Add((initialState, initialState));
        queue.Enqueue((initialState, initialState));

        long maxPressure = 0;
        (State, State) maxState;

        while (queue.TryDequeue(out var state))
        {
            var (state1, state2) = state;

            var pressure = Math.Max(state1.Pressure + state2.Pressure, maxPressure);
            if (pressure > maxPressure)
            {
                maxPressure = pressure;
                maxState = state;
            }

            var newState1 = new List<State>();
            foreach (var state1Neighbor in graph.OutEdges(state1.Valve))
            {
                var state1TimeRemaining = state1.TimeRemaining - (int)func(state1Neighbor);
                if (state1TimeRemaining < 0) continue;

                var state1Visited = state1.Visited.Contains(state1Neighbor.Target);
                var state2Visited = state2.Visited.Contains(state1Neighbor.Target) || state2.Valve == state1Neighbor.Target;
                if (state1TimeRemaining > 0 && !state1Visited && !state2Visited)
                {
                    newState1.Add(new State
                    {
                        Valve = state1Neighbor.Target,
                        Pressure = state1.Pressure + (state1TimeRemaining * state1Neighbor.Target.Rate),
                        TimeRemaining = state1TimeRemaining,
                        Visited = state1.Visited.Concat(new[] { state1.Valve }).ToHashSet()
                    });
                }
            }

            if (newState1.Count == 0) newState1.Add(state1);


            var newState2 = new List<State>();
            foreach (var state2Neighbor in graph.OutEdges(state2.Valve))
            {
                var state2TimeRemaining = state2.TimeRemaining - (int)func(state2Neighbor);
                if (state2TimeRemaining < 0) continue;

                var state1Visited = state1.Visited.Contains(state2Neighbor.Target) || state1.Valve == state2Neighbor.Target;
                var state2Visited = state2.Visited.Contains(state2Neighbor.Target);
                if (state2TimeRemaining > 0 && !state1Visited && !state2Visited)
                {
                    newState2.Add(new State
                    {
                        Valve = state2Neighbor.Target,
                        Pressure = state2.Pressure + (state2TimeRemaining * state2Neighbor.Target.Rate),
                        TimeRemaining = state2TimeRemaining,
                        Visited = state2.Visited.Concat(new[] { state2.Valve }).ToHashSet()
                    });
                }
            }

            if (newState2.Count == 0) newState1.Add(state2);

            var newState = (
                from s1 in newState1
                from s2 in newState2
                where s1.Valve != s2.Valve
                select (s1, s2)
            ).ToList();

            foreach (var s in newState)
            {
                var (s1, s2) = s;
                if (hash.Contains(s)) continue;
                if (state1.Visited.Contains(s2.Valve) ||
                    state2.Visited.Contains(s1.Valve) ||
                    state1.Valve == s2.Valve ||
                    state2.Valve == s1.Valve ||
                    s1.Visited.Contains(s2.Valve) ||
                    s2.Visited.Contains(s1.Valve))
                    continue;

                hash.Add(s);
                queue.Enqueue(s);
            }
        }

        return maxPressure;
    }

    DelegateVertexAndEdgeListGraph<Valve, EquatableEdge<Valve>> graph = null!;

    protected override long InternalSolve()
    {
        this.graph = CreateGraph(ParseInput());

        var pairs = (
            from n1 in graph.Vertices.Where(_ => StartNode(_) || ImportantNode(_))
            from n2 in graph.Vertices.Where(_ => StartNode(_) || ImportantNode(_))
            where n1 != n2
            where n2.Name != "AA"
            select (start: n1, end: n2)
        ).Select(_ => ShortestPath(_.start)(_.end).ToList()).ToList();

        return GetElephantPressure(pairs);
    }

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
}