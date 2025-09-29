// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 3


using System.Collections;

namespace NoConcat
{

    // The first successful version of the deque developed in chapter 3,
    // before adding concatenation.

    public interface IDeque<T> : IEnumerable<T>
    {
        IDeque<T> PushLeft(T item);
        IDeque<T> PushRight(T item);
        IDeque<T> PopLeft();
        IDeque<T> PopRight();
        T Left();
        T Right();
        bool IsEmpty { get; }
    }

    public sealed class Deque<T> : IDeque<T>
    {
        // Listing 3.4: The mini-deque
        private interface IMini : IEnumerable<T>
        {
            public int Size { get; }
            public IMini PushLeft(T item);
            public IMini PushRight(T item);
            public IMini PopLeft();
            public IMini PopRight();
            public T Left();
            public T Right();
        }
        private record One(T item1) : IMini
        {
            public int Size => 1;
            public IMini PushLeft(T item) => new Two(item, item1);
            public IMini PushRight(T item) => new Two(item1, item);
            public IMini PopLeft() => throw new InvalidOperationException();
            public IMini PopRight() => throw new InvalidOperationException();
            public T Left() => item1;
            public T Right() => item1;
            public IEnumerator<T> GetEnumerator()
            {
                yield return item1;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        private record Two(T item1, T item2) : IMini
        {
            public int Size => 2;
            public IMini PushLeft(T item) => new Three(item, item1, item2);
            public IMini PushRight(T item) => new Three(item1, item2, item);
            public IMini PopLeft() => new One(item2);
            public IMini PopRight() => new One(item1);
            public T Left() => item1;
            public T Right() => item2;
            public IEnumerator<T> GetEnumerator()
            {
                yield return item1;
                yield return item2;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        private record Three(T item1, T item2, T item3) : IMini
        {
            public int Size => 3;
            public IMini PushLeft(T item) =>
                new Four(item, item1, item2, item3);
            public IMini PushRight(T item) =>
                new Four(item1, item2, item3, item);
            public IMini PopLeft() => new Two(item2, item3);
            public IMini PopRight() => new Two(item1, item2);
            public T Left() => item1;
            public T Right() => item3;
            public IEnumerator<T> GetEnumerator()
            {
                yield return item1;
                yield return item2;
                yield return item3;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        private record Four(T item1, T item2, T item3, T item4) : IMini
        {
            public int Size => 4;
            public IMini PushLeft(T item) =>
                throw new InvalidOperationException();
            public IMini PushRight(T item) =>
                throw new InvalidOperationException();
            public IMini PopLeft() => new Three(item2, item3, item4);
            public IMini PopRight() => new Three(item1, item2, item3);
            public T Left() => item1;
            public T Right() => item4;
            public IEnumerator<T> GetEnumerator()
            {
                yield return item1;
                yield return item2;
                yield return item3;
                yield return item4;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        private sealed class EmptyDeque : IDeque<T>
        {
            public IDeque<T> PushLeft(T item) => new SingleDeque(item);
            public IDeque<T> PushRight(T item) => new SingleDeque(item);
            public IDeque<T> PopLeft() => throw new InvalidOperationException();
            public IDeque<T> PopRight() => throw new InvalidOperationException();
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
                new Deque<T>(new One(newItem), Deque<IMini>.Empty, new One(item));
            public IDeque<T> PushRight(T newItem) =>
                new Deque<T>(new One(item), Deque<IMini>.Empty, new One(newItem));
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

        // Listing 3.5: The deque of minis

        private readonly IMini left;
        private readonly IDeque<IMini> middle;
        private readonly IMini right;

        private Deque(IMini left, IDeque<IMini> middle, IMini right)
        {
            this.left = left;
            this.middle = middle;
            this.right = right;
        }

        public IDeque<T> PushLeft(T value) =>
            left.Size < 4 ?
                new Deque<T>(left.PushLeft(value), middle, right) :
                new Deque<T>(
                    new Two(value, left.Left()),
                    middle.PushLeft(left.PopLeft()),
                    right);

        public IDeque<T> PushRight(T value) =>
            right.Size < 4 ?
                new Deque<T>(left, middle, right.PushRight(value)) :
                new Deque<T>(
                    left,
                    middle.PushRight(right.PopRight()),
                    new Two(right.Right(), value));

        public IDeque<T> PopLeft()
        {
            if (left.Size > 1)
                return new Deque<T>(left.PopLeft(), middle, right);
            if (!middle.IsEmpty)
                return new Deque<T>(middle.Left(), middle.PopLeft(), right);
            if (right.Size > 1)
                return new Deque<T>(new One(right.Left()), middle, right.PopLeft());
            return new SingleDeque(right.Left());
        }

        public IDeque<T> PopRight()
        {
            if (right.Size > 1)
                return new Deque<T>(left, middle, right.PopRight());
            if (!middle.IsEmpty)
                return new Deque<T>(left, middle.PopRight(), middle.Right());
            if (left.Size > 1)
                return new Deque<T>(left.PopRight(), middle, new One(left.Right()));
            return new SingleDeque(left.Right());
        }

        public T Left() => left.Left();
        public T Right() => right.Right();
        public bool IsEmpty => false;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in left)
                yield return item;
            foreach (var mini in middle)
                foreach (var item in mini)
                    yield return item;
            foreach (var item in right)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}