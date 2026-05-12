// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public interface IDistribution<T>
{
    T Sample();
}

public interface IDiscreteDistribution<T> : IDistribution<T>
{
    IEnumerable<T> Support();
    int Weight(T t);
}

public interface IWeightedDistribution<T> : IDistribution<T>
{
    double Weight(T value);
}

public interface IContinuousDistribution : IWeightedDistribution<double>
{ }
