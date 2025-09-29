// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 6
static partial class Extensions
{
    public static string Newlines<T>(this IEnumerable<T> seq) =>
        string.Join("\n", seq);

    public static string Concat<T>(this IEnumerable<T> items) =>
        string.Join("", items);

    public static string Space<T>(this IEnumerable<T> items) =>
        string.Join(" ", items);
}
