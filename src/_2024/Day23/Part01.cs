using QuikGraph;

namespace _2024.Day23;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var graph = input
            .SplitLines()
            .Select(s => s.Split('-'))
            .Select(s => new Edge<string>(s[0], s[1]))
            .ToBidirectionalGraph<string, Edge<string>>();

        var paths = new List<List<string>>();
        foreach (var vertex in graph.Vertices)
        {
            var states = new Stack<(string, List<string>)>();
            states.Push((vertex, [vertex]));

            while (states.TryPop(out var state))
            {
                var (v, path) = state;

                if (path.Count == 3)
                {
                    if (graph.OutEdges(v).Any(e => e.Target == vertex))
                        paths.Add(path);
                    
                    continue;
                }

                var outEdges = graph.OutEdges(v).Select(e => e.Target);
                var inEdges = graph.InEdges(v).Select(e => e.Source);
                
                var neighbors = outEdges
                    .Concat(inEdges)
                    .Where(e => !path.Contains(e));

                foreach (var neighbor in neighbors)
                {
                    states.Push((neighbor, [..path, neighbor]));
                }
            }
        }

        var lans = paths.DistinctBy(p => string.Join("-", p.OrderBy()));

        var tlans = lans.Count(l => l.Any(c => c[0] == 't'));
        
        return tlans;
    }
}
