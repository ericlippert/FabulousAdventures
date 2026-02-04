// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

using System.Collections.Immutable;

// Listing 8.4

static class SimpleGraphColoring
{
    public static ImmutableDictionary<N, C>? ColorGraph<N, C>(
        this ImGraph<N> graph, 
        ImmutableHashSet<C> colors)
        where N : notnull where C : notnull
    {
        if (graph.IsEmpty) 
            return ImmutableDictionary<N, C>.Empty;
        N? last = graph.Nodes
            .Where(n => graph.Edges(n).Count < colors.Count)
            .FirstOrDefault();
        if (last is null)
            return null;
        var coloring = graph.RemoveNode(last).ColorGraph(colors);
        if (coloring is null)
            return null;
        var usedColors = graph.Edges(last).Select(n => coloring[n]);
        var lastColor = colors.Except(usedColors).First();
        return coloring.Add(last, lastColor);
    }
}
