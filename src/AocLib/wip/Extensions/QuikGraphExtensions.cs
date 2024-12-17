using QuikGraph;
using QuikGraph.Algorithms.Cliques;
using QuikGraph.Algorithms.Services;
using System.Diagnostics.CodeAnalysis;

namespace AocLib;

public static class GraphExtensions
{
    public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary)
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        where TValue : IEnumerable<TEdge> =>
        dictionary.ToVertexAndEdgeListGraph(kv => kv.Value);

    public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(
        this IDictionary<TVertex, TValue> dictionary,
        Converter<KeyValuePair<TVertex, TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
        where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
        new(
            dictionary.Keys,
            delegate (TVertex key, out IEnumerable<TEdge>? edges)
            {
                if (dictionary.TryGetValue(key, out TValue? value))
                {
                    edges = keyValueToOutEdges(new KeyValuePair<TVertex, TValue>(key, value));
                    return true;
                }

                edges = null;
                return false;
            });
}

public class BronKerboshMaximumCliqueAlgorithm<TVertex, TEdge> : MaximumCliqueAlgorithmBase<TVertex, TEdge>
    where TEdge : IEdge<TVertex>
{
    List<ISet<TVertex>> cliques = [];

    public BronKerboshMaximumCliqueAlgorithm(
        IAlgorithmComponent host,
        [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
        : base(host, visitedGraph)
    {
    }

    public BronKerboshMaximumCliqueAlgorithm(
        [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
        : base(visitedGraph)
    {
    }

    public IList<ISet<TVertex>> Cliques => cliques;

    protected override void InternalCompute()
    {
        var candidates = new HashSet<TVertex>(VisitedGraph.Vertices);
        var excluded = new HashSet<TVertex>();
        var visited = new HashSet<TVertex>();

        FindCliques(candidates, excluded, visited, cliques);
    }

    void FindCliques(
        HashSet<TVertex> candidates,
        HashSet<TVertex> excluded,
        HashSet<TVertex> visited,
        List<ISet<TVertex>> cliques)
    {
        if (candidates.Count == 0 && excluded.Count == 0)
        {
            cliques.Add(new HashSet<TVertex>(visited));
            return;
        }

        var pivot = candidates
            .Concat(excluded)
            .OrderByDescending(v => VisitedGraph.AdjacentDegree(v))
            .First();

        var neighbors = new HashSet<TVertex>(VisitedGraph.AdjacentEdges(pivot).Where(_ => _.Source != null).Select(_ => _.Source!.Equals(pivot) ? _.Target : _.Source));
        var remainingCandidates = new HashSet<TVertex>(candidates.Where(v => neighbors.Contains(v)));
        var remainingExcluded = new HashSet<TVertex>(excluded.Where(v => neighbors.Contains(v)));

        foreach (var candidate in remainingCandidates)
        {
            var newCandidates = new HashSet<TVertex>(candidates.Where(v => VisitedGraph.AdjacentDegree(v) > VisitedGraph.AdjacentDegree(candidate)));
            var newExcluded = new HashSet<TVertex>(excluded.Where(v => VisitedGraph.AdjacentDegree(v) > VisitedGraph.AdjacentDegree(candidate)));
            var newVisited = new HashSet<TVertex>(visited) { candidate };

            FindCliques(newCandidates, newExcluded, newVisited, cliques);

            candidates.Remove(candidate);
            excluded.Add(candidate);
        }
    }
}
