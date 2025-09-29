// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 1

// An immutable linked list

// There are some good design decisions here but also some 
// questionable ones!

sealed record LinkedList<T>(T? Value, LinkedList<T>? Tail)
{
    public static LinkedList<T> Empty = new(default, null);
    // C# infers "Empty = new LinkedList<T>(default(T), null)"

    public LinkedList<T> Push(T value) => new(value, this);
    public bool IsEmpty => this == Empty;
    // Did you catch why this is correct but not as efficient
    // as it could be? "IsEmpty => Tail == null" would be better.

    public override string ToString()
    {
        var s = "";
        for (var list = this; !list.IsEmpty; list = list.Tail!)
            s += list.Value + " ";
        // This is "accidentally quadradtic". A StringBuilder would
        // be the better choice.
        return s;
    }
    public LinkedList<T> Reverse()
    {
        var result = Empty;
        for (var list = this; !list.IsEmpty; list = list.Tail!)
            result = result.Push(list.Value!);
        return result;
    }
}


