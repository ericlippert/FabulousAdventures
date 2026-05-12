// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

sealed class CategoricalBuilder<T> where T : notnull
{
    private Dictionary<T, int> weights = [];
    public void Add(T t)
    {
        weights[t] = weights.GetValueOrDefault(t) + 1;
    }
    public IDistribution<T> ToDistribution()
    {
        var keys = weights.Keys.ToList();
        return keys.ToCategorical(keys.Select(k => weights[k]));
    }
}
