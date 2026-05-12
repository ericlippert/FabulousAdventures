// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public sealed class Projected<T, R> : IDiscreteDistribution<R>
    where R : notnull
{
    private readonly IDiscreteDistribution<T> underlying;
    private readonly Func<T, R> projection;
    private readonly Dictionary<R, int> weights;

    public static IDiscreteDistribution<R> Distribution(
      IDiscreteDistribution<T> underlying,
      Func<T, R> projection)
    {
        var weights = underlying.Support()
            .GroupBy(projection, underlying.Weight)
            .ToDictionary(g => g.Key, g => g.Sum());
        int gcd = weights.Values.GCD();
        foreach (var key in weights.Keys)
            weights[key] /= gcd;
        if (weights.Count == 1)
            return Singleton<R>.Distribution(weights.Keys.First());
        return new Projected<T, R>(underlying, projection, weights);
    }

    private Projected(IDiscreteDistribution<T> underlying,
        Func<T, R> projection, Dictionary<R, int> weights)
    {
        this.underlying = underlying;
        this.projection = projection;
        this.weights = weights;
    }

    public R Sample() => projection(underlying.Sample());
    public IEnumerable<R> Support() => weights.Keys;
    public int Weight(R r) => weights.GetValueOrDefault(r, 0);
}
