// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

using System.Collections.Immutable;

// Listing 8.2

readonly struct ImGraph<N> where N : notnull
{
    public static ImGraph<N> Empty = new(ImMulti<N, N>.Empty);
    private readonly ImMulti<N, N> nodes;
    private ImGraph(ImMulti<N, N> ns) => this.nodes = ns;
    public bool IsEmpty => nodes.IsEmpty;
    public bool HasNode(N n) => nodes.ContainsKey(n);
    public bool HasEdge(N n1, N n2) => nodes.HasValue(n1, n2);

    public IEnumerable<N> Nodes => nodes.Keys;

    public ImmutableHashSet<N> Edges(N n) => nodes[n];

    public ImGraph<N> AddNode(N n) =>
        HasNode(n) ? this : new(nodes.SetEmpty(n));

    public ImGraph<N> AddEdge(N n1, N n2) =>
        new(nodes.Add(n1, n2).Add(n2, n1));

    public ImGraph<N> RemoveEdge(N n1, N n2) =>
        new(nodes.Remove(n1, n2).Remove(n2, n1));

    public ImGraph<N> RemoveNode(N n)
    {
        var result = this;
        foreach (var n2 in Edges(n))
            result = result.RemoveEdge(n, n2);
        return new(result.nodes.Remove(n));
    }

    public ImGraph<N> AddEdges(N n1, IEnumerable<N> ns)
    {
        var result = this;
        foreach (var n2 in ns)
            result = result.AddEdge(n1, n2);
        return result;
    }

    public ImGraph<N> AddClique(IList<N> ns)
    {
        var result = this;
        for (int i = 0; i < ns.Count; i += 1)
            for (int j = i + 1; j < ns.Count; j += 1)
                result = result.AddEdge(ns[i], ns[j]);
        return result;
    }
}