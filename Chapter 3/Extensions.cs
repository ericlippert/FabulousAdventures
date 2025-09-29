// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 3
static class Extensions
{
    public static IDeque<T> PushRightMany<T>(
        this IDeque<T> deque, IEnumerable<T> items) =>
        items.Aggregate(deque, (d, item) => d.PushRight(item));
}