using MoreLinq;
using QuikGraph;
using Spectre.Console;

namespace _2024.Day23;

public class Part02 : PuzzleSolver<string>
{
    protected override string InternalSolve()
    {
        var graph = input
            .SplitLines()
            .Select(s => s.Split('-'))
            .Select(s => new Edge<string>(s[0], s[1]))
            .ToUndirectedGraph<string, Edge<string>>();

        var maxClique = new HashSet<string>();
        foreach (var vertex in graph.Vertices)
        {
            var clique = new HashSet<string>() { vertex };
            foreach (var neighbor in graph.AdjacentVertices(vertex))
            {
                if (clique.All(v => graph.ContainsEdge(v, neighbor)))
                {
                    clique.Add(neighbor);
                }
            }

            if (clique.Count > maxClique.Count)
                maxClique = clique;
        }

        return string.Join(",", maxClique.OrderBy(s => s));
    }
}