// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public sealed class Singleton<T> : IDiscreteDistribution<T>
{
    private readonly T t;
    public static Singleton<T> Distribution(T t) => new(t);
    private Singleton(T t) => this.t = t;
    public T Sample() => t;
    public IEnumerable<T> Support() => [t];
    public int Weight(T t) =>
        EqualityComparer<T>.Default.Equals(this.t, t) ? 1 : 0; 
}
