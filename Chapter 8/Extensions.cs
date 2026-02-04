// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

static class Extensions
{
    public static string Comma<T>(this IEnumerable<T> items) =>
        string.Join(',', items);
}
