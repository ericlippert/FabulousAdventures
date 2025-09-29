// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

// A covariant immutable stack

namespace Covariance
{
    using System.Collections;
    static class CovImmutableStack
    {
        public static void SampleCode()
        {
            Console.WriteLine("A covariant immutable stack");

            // By making Push a static method of the class and
            // adding an extension method to maintain our fluent user
            // interface, we can create a covariant version of the 
            // immutable stack.

            IImStack<Tiger> s1 = ImStack<Tiger>.Empty;
            IImStack<Tiger> s2 = s1.Push(new Tiger());
            IImStack<Tiger> s3 = s2.Push(new Tiger());
            IImStack<Animal> s4 = s3; // Legal because of covariance.
            IImStack<Animal> s5 = s4.Push(new Giraffe());

            Console.WriteLine(s5.Bracket());
        }
    }

    class Animal
    {
        public override string ToString() => this.GetType().Name;
    }
    class Tiger : Animal { }
    class Giraffe : Animal { }

    interface IImStack<out T> : IEnumerable<T>
    {
        T Peek();
        IImStack<T> Pop();
        bool IsEmpty { get; }
    }

    class ImStack<T> : IImStack<T>
    {
        private class EmptyStack : IImStack<T>
        {
            public EmptyStack() { }
            public T Peek() => throw new InvalidOperationException();
            public IImStack<T> Pop() => throw new InvalidOperationException();
            public bool IsEmpty => true;
            public IEnumerator<T> GetEnumerator()
            {
                yield break;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public static IImStack<T> Empty { get; } = new EmptyStack();

        private readonly T item;
        private readonly IImStack<T> tail;
        private ImStack(T item, IImStack<T> tail)
        {
            this.item = item;
            this.tail = tail;
        }
        public static IImStack<T> Push(T item, IImStack<T> tail) => new ImStack<T>(item, tail);
        public T Peek() => item;
        public IImStack<T> Pop() => tail;
        public bool IsEmpty => false;
        public IEnumerator<T> GetEnumerator()
        {
            for (IImStack<T> s = this; !s.IsEmpty; s = s.Pop())
                yield return s.Peek();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    static class Extensions
    {
        public static IImStack<T> Push<T>(this IImStack<T> stack, T item) => 
            ImStack<T>.Push(item, stack);
    }
}