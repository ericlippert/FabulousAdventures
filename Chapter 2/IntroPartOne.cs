// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Introduction to Part One

static class IntroPartOne
{
    public static void SampleCode()
    {
        // Modification of a variable that is closed over by a
        // lambda or query is a common error in many modern programming
        // languages that allow capture of *variables* instead of
        // *values*. Here's an illustration. Let's make three filters,
        // where we intend each to filter the 1s, 2s and 3s respectively.
        Console.WriteLine("Intro to Part One");
        int[] data = { 1, 2, 3, 1, 2, 1, 1, 2, 2, 2 };
        var queries = new List<IEnumerable<int>>();
        for (int i = 1; i < 4; i += 1)
            queries.Add(from k in data where k == i select k);
        
        // We've just bound the query to the variable i,
        // not to the value i had when the query was created.

        foreach (var query in queries)
            Console.Write(query.Count()); 
        // 0 0 0, because i is now 4 and there are no 4s.

        Console.WriteLine();

        queries.Clear();
        for (int i = 1; i < 4; i += 1)
        {
            int j = i;
            queries.Add(from k in data where k == j select k);
        }

        // We've just bound the query to the variable j,
        // and there is a fresh local variable created every time
        // through the loop.

        foreach (var query in queries)
            Console.Write(query.Count());
        // 4 5 1, four 1s, five 2s, one 3, as expected.

        Console.WriteLine();

        // The moral of the story is that variables varying can trip you
        // up in strange ways. **The less things change, the more they stay
        // the same.** It's easier to reason about programs where fewer
        // things are changing all the time. Understanding programs is 
        // hard enough already; I always try to simplify programs by
        // restricting what can change.
    }
}
