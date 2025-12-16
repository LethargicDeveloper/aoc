
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;
using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day11;

public partial class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var edges = input
            .SplitLines()
            .Select(line =>
            {
                var parts = Regex.Split(line, ":");
                var name = parts[0];
                var outputs = parts[1].S(' ').Order().ToList();

                return (name, outputs);
            })
            .SelectMany(kv => kv.outputs.Select(o => new Edge<string>(kv.name, o)))
            .ToList();

        var vertices = new HashSet<string>(edges.SelectMany(e => new[] { e.Source, e.Target }));

        var graph = new DelegateVertexAndEdgeListGraph<string, Edge<string>>(vertices, (v, out result) =>
        {
            result = edges.Where(e => e.Source == v);
            return true;
        });

        var topo = graph.TopologicalSort().ToList();
        
        return PathsBetween("svr", "fft") * PathsBetween("fft", "dac") * PathsBetween("dac", "out");
        
        long PathsBetween(string source, string target)
        {
            var counts = topo.ToDictionary(v => v, v => 0L);
            counts[source] = 1;
            
            foreach (var node in topo)
            foreach (var edge in graph.OutEdges(node))
                counts[edge.Target] += counts[node];

            return counts[target];
        }
    }
}