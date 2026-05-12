// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public sealed class DiscreteUniform : IDiscreteDistribution<int>
{
    private readonly Random rnd = new();
    private readonly int max;
    public static DiscreteUniform Distribution(int max)
    {
        if (max <= 0)
            throw new InvalidOperationException();
        return new(max);
    }
    private DiscreteUniform(int max) => this.max = max;
    public int Sample() => rnd.Next(max);
    public IEnumerable<int> Support() => Enumerable.Range(0, max);
    public int Weight(int t) => 0 <= t && t < max ? 1 : 0;
}
