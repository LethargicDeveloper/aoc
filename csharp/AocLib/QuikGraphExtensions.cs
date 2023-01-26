namespace QuikGraph;

public static class GraphExtensions
{
    public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary)
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        where TValue : IEnumerable<TEdge> =>
        ToVertexAndEdgeListGraph(dictionary, kv => kv.Value);

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
