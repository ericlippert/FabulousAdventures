// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

using static System.Math;
public sealed class Binomial : IWeightedDistribution<int>
{
    private int trials;
    private double prob;
    private IDistribution<bool> flip;

    public static Binomial Distribution(int trials, double prob) =>
        new(trials, prob);

    private Binomial(int trials, double prob)
    {
        this.trials = trials;
        this.prob = prob;
        this.flip = new Flip<bool>(true, false, prob);
    }

    public double Weight(int successes) =>
        Pow(prob, successes) *
        Pow(1.0 - prob, trials - successes) *
        (double)trials.Choose(successes);

    public int Sample() => flip.Samples().Take(trials).Count(x => x);
}
