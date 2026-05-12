// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
public sealed class Empty<T> : IDiscreteDistribution<T>
{
    public static readonly Empty<T> Distribution = new();
    private Empty() { }
    public T Sample() => throw new InvalidOperationException();
    public IEnumerable<T> Support() => Enumerable.Empty<T>();
    public int Weight(T t) => 0;
}
