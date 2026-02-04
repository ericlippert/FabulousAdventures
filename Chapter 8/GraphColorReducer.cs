// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

// Listing 8.6
sealed class GraphColorReducer<N, C> : IReducer<N, C> 
    where N : notnull where C : notnull
{
    private readonly ImGraph<N> graph;

    public GraphColorReducer(ImGraph<N> graph) => this.graph = graph;

    private (ImMulti<N, C>, bool) ReduceOnce(ImMulti<N, C> attempt)
    {
        bool progress = false;
        var result = attempt;
        foreach (N n1 in attempt.Keys.Where(k => attempt[k].Count == 1))
        {
            C c = attempt[n1].Single();
            var elim = graph.Edges(n1).Where(n => attempt.HasValue(n, c));
            foreach (N n2 in elim)
            {
                result = result.Remove(n2, c);
                progress = true;
            }
        }
        return (result, progress);
    }

    public ImMulti<N, C> Reduce(ImMulti<N, C> attempt)
    {
        bool progress;
        do
            (attempt, progress) = ReduceOnce(attempt);
        while (progress);
        return attempt;
    }
}
