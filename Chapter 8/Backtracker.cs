// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

// Listing 8.7

sealed class Backtracker<N, C> where N : notnull where C : notnull
{
    private readonly IReducer<N, C> reducer;

    public Backtracker(IReducer<N, C> reducer) => this.reducer = reducer;

    public  IEnumerable<ImMulti<N, C>> Solve(ImMulti<N,C> attempt)
    {
        attempt = reducer.Reduce(attempt);
        if (attempt.Keys.Any(k => attempt[k].IsEmpty))
            return [];
        if (attempt.Keys.All(k => attempt[k].Count == 1))
            return [attempt];
        // There must be at least one key with multiple values if we're neither
        // broken nor solved. Pick any of them, doesn't matter which.
        N guessKey = attempt.Keys.Where(k => attempt[k].Count > 1).First();
        return attempt[guessKey].SelectMany(v => Solve(attempt.SetSingle(guessKey, v)));
    }
}
