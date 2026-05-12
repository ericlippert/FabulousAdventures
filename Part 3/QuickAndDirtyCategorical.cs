// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness


static class QuickAndDirtyCategorical
{
    public static IDiscreteDistribution<T> MakeCategorical<T>(
        IList<T> categories, IList<int> weights) where T : notnull
    {
        List<T> values = [];
        for (int i = 0; i < categories.Count; i += 1)
        {
            T category = categories[i];
            int weight = weights[i];
            for (int j = 0; j < weight; j += 1)
                values.Add(category);
        }
        return DiscreteUniform.Distribution(values.Count)
            .Project(s => values[s]);
    }
}