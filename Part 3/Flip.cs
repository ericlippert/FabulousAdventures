// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
public sealed record class Flip<T>(T Heads, T Tails, double PHeads) :
    IDistribution<T>
{
    public T Sample() =>
        ContinuousUniform.Standard.Sample() <= PHeads ? Heads : Tails;
}
