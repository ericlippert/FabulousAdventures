// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

sealed class MarkovBuilder<T> where T : notnull
{
    private CategoricalBuilder<T> initial = new();
    private readonly Dictionary<T, CategoricalBuilder<T>> transitions = new();
    public void AddInitial(T t)
    {
        initial.Add(t);
    }
    public void AddTransition(T t1, T t2)
    {
        if (!transitions.ContainsKey(t1))
            transitions[t1] = new();
        transitions[t1].Add(t2);
    }
    public Markov<T> ToDistribution()
    {
        var init = initial.ToDistribution();
        var trans = transitions.ToDictionary(
          kv => kv.Key,
          kv => kv.Value.ToDistribution());
        return Markov<T>.Distribution(init,
          k => trans.GetValueOrDefault(k, Empty<T>.Distribution));
    }
}
