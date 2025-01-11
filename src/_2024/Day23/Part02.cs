using QuikGraph;

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

        var states = new Stack<(string, List<string>, List<List<string>>)>();
        foreach (var vertex in graph.Vertices)
            states.Push((vertex, [vertex], [graph
                .AdjacentEdges(vertex)
                .Select(e => e.Target == vertex ? e.Source : e.Target)
                .ToList()]));

        var paths = new List<List<string>>();
        while (states.TryPop(out var state))
        {
            var (vertex, path, adj) = state;

            var neighbors = adj[^1]
                .Where(n => !path.Contains(n))
                .Where(n => adj.All(a => a.Contains(n)))
                .ToList();

            if (neighbors.Count == 0)
            {
                paths.Add(path);
            }
            
            foreach (var neighbor in neighbors)
            {
                states.Push((neighbor, [..path, neighbor], [..adj, graph
                    .AdjacentEdges(neighbor)
                    .Select(e => e.Target == vertex ? e.Source : e.Target)
                    .ToList()]));
            }
        }
       
        var longest = paths.MaxBy(p => p.Count);
        
        return string.Join(",", longest!.OrderBy());
    }
}
