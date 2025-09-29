// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

// Handy extension methods
static class Extensions
{
    public static string Comma<T>(this IEnumerable<T> items) => 
        string.Join(',', items);
    public static string Bracket<T>(this IEnumerable<T> items) => 
        "[" + items.Comma() + "]";

    // Original version of Reverse
    //
    // public static IImStack<T> Reverse<T>(this IImStack<T> stack)
    // {
    //    var result = ImStack<T>.Empty;
    //    for (; !stack.IsEmpty; stack = stack.Pop())
    //        result = result.Push(stack.Peek());
    //    return result;
    // }
    //
    // Refactored into ReverseOnto helper:

    public static IImStack<T> ReverseOnto<T>(this IImStack<T> stack, IImStack<T> tail)
    {
        var result = tail;
        for (; !stack.IsEmpty; stack = stack.Pop())
            result = result.Push(stack.Peek());
        return result;
    }
    public static IImStack<T> Reverse<T>(this IImStack<T> stack) =>
        stack.ReverseOnto(ImStack<T>.Empty);
    public static IImStack<T> Concatenate<T>(this IImStack<T> xs, IImStack<T> ys) =>
        ys.IsEmpty ? xs : xs.Reverse().ReverseOnto(ys);
    public static IImStack<T> Append<T>(this IImStack<T> stack, T item) =>
        stack.Concatenate(ImStack<T>.Empty.Push(item));
}