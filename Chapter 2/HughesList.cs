// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

using System.Collections;

static class HughesList
{
    public static void SampleCode()
    {
        Console.WriteLine("The Hughes list");
        Func<int, int, int> adder = (x, y) => x + y;
        Console.WriteLine(adder(4, 3));
        Func<int, Func<int, int>> curried = x => y => x + y;
        Func<int, int> add4 = curried(4);
        Console.WriteLine(add4(3));

        var s = ImStack<int>.Empty.Push(2).Push(3).Push(4);
        var hl432 = HList<int>.FromStack(s);
        var hl = hl432.Push(5).Append(1).Concatenate(hl432).Append(0);
        Console.WriteLine(hl.Bracket());
        Console.WriteLine(HList<int>.Reverse(hl.ToStack()).Bracket());
    }
}


// Listing 2.14: The Hughes list
struct HList<T> : IEnumerable<T>
{
    delegate IImStack<T> Concat(IImStack<T> stack);
    private readonly Concat c;
    private HList(Concat c) => this.c = c;
    private static HList<T> Make(Concat c) => new HList<T>(c);
    public static HList<T> Empty { get; } = Make(stack => stack);
    public bool IsEmpty => ReferenceEquals(c, Empty.c);
    public static HList<T> FromStack(IImStack<T> fromStack) =>
        fromStack.IsEmpty ?
            Empty :
            Make(stack => fromStack.Concatenate(stack));
    public static HList<T> Reverse(IImStack<T> fromStack) =>
    fromStack.IsEmpty ?
        Empty :
        Make(stack => fromStack.ReverseOnto(stack));
    public IImStack<T> ToStack() => c(ImStack<T>.Empty);
    public T Peek() => ToStack().Peek();
    public HList<T> Pop() => FromStack(ToStack().Pop());
    private static HList<T> Concatenate(HList<T> hl1, HList<T> hl2) =>
        hl1.IsEmpty ? hl2 : Make(stack => hl1.c(hl2.c(stack)));
    public static HList<T> Single(T item) => Make(stack => stack.Push(item));
    public HList<T> Push(T item) => Concatenate(Single(item), this);
    public HList<T> Append(T item) => Concatenate(this, Single(item));
    public HList<T> Concatenate(HList<T> hl) => Concatenate(this, hl);
    public IEnumerator<T> GetEnumerator() => ToStack().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
