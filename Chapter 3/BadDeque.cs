// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 3

using System.Collections;

namespace BadDeque
{
    using NoConcat;
    // A poor implementation of a deque; the naive implementation is correct but
    // completely lacks persistence and is very inefficient.

    public sealed class Deque<T> : IDeque<T>
    {
        private sealed class EmptyDeque : IDeque<T>
        {
            public IDeque<T> PushLeft(T item) => new SingleDeque(item);
            public IDeque<T> PushRight(T item) => new SingleDeque(item);
            public IDeque<T> PopLeft() =>
                throw new InvalidOperationException();
            public IDeque<T> PopRight() =>
                throw new InvalidOperationException();
            public T Left() => throw new InvalidOperationException();
            public T Right() => throw new InvalidOperationException();
            public bool IsEmpty => true;
            public IEnumerator<T> GetEnumerator()
            {
                yield break;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public static IDeque<T> Empty { get; } = new EmptyDeque();
        private record SingleDeque(T item) : IDeque<T>
        {
            public IDeque<T> PushLeft(T newItem) =>
                new Deque<T>(newItem, Empty, item);
            public IDeque<T> PushRight(T newItem) =>
                new Deque<T>(item, Empty, newItem);
            public IDeque<T> PopLeft() => Empty;
            public IDeque<T> PopRight() => Empty;
            public T Left() => item;
            public T Right() => item;
            public bool IsEmpty => false;
            public IEnumerator<T> GetEnumerator()
            {
                yield return item;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private readonly T left;
        private readonly IDeque<T> middle;
        private readonly T right;

        private Deque(T left, IDeque<T> middle, T right)
        {
            this.left = left;
            this.middle = middle;
            this.right = right;
        }

        public IDeque<T> PushLeft(T item) =>
            new Deque<T>(item, middle.PushLeft(left), right);
        public IDeque<T> PushRight(T item) =>
            new Deque<T>(left, middle.PushRight(right), item);
        public IDeque<T> PopLeft() =>
            middle.IsEmpty ?
                new SingleDeque(right) :
                new Deque<T>(middle.Left(), middle.PopLeft(), right);
        public IDeque<T> PopRight() =>
            middle.IsEmpty ?
                new SingleDeque(left) :
                new Deque<T>(left, middle.PopRight(), middle.Right());
        public T Left() => left;
        public T Right() => right;
        public bool IsEmpty => false;

        public IEnumerator<T> GetEnumerator()
        {
            yield return left;
            foreach (var item in middle)
                yield return item;
            yield return right;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}