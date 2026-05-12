// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
public sealed class Metropolis : IContinuousDistribution
{
    private readonly IEnumerator<double> enumerator;
    private readonly Func<double, double> target;
    public static Metropolis Distribution(
      Func<double, double> target,
      IDistribution<double> initial,
      Func<double, IDistribution<double>> proposal)
    {
        var markov = Markov<double>.Distribution(initial, transition);
        return new Metropolis(target, markov.Sample().GetEnumerator());

        IDistribution<double> transition(double current)
        {
            double proposed = proposal(current).Sample();
            return new Flip<double>(proposed, current,
                target(proposed) / target(current));
        }
    }
    private Metropolis(
        Func<double, double> target, IEnumerator<double> enumerator)
    {
        this.enumerator = enumerator;
        this.target = target;
    }
    public double Sample()
    {
        enumerator.MoveNext();
        return enumerator.Current;
    }
    public double Weight(double d) => target(d);
}

