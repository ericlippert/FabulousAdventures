// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

sealed class Markov<T> : IDistribution<IEnumerable<T>>
{
    private readonly IDistribution<T> initial;
    private readonly Func<T, IDistribution<T>> transition;

    public static Markov<T> Distribution(
        IDistribution<T> initial,
        Func<T, IDistribution<T>> transition) =>
        new(initial, transition);

    private Markov(
      IDistribution<T> initial,
      Func<T, IDistribution<T>> transition)
    {
        this.initial = initial;
        this.transition = transition;
    }
    public IEnumerable<T> Sample()
    {
        var distribution = initial;
        while (distribution is not Empty<T>)
        {
            var state = distribution.Sample();
            yield return state;
            distribution = transition(state);
        }
    }
}
