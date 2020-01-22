using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public readonly struct ArrayEnumerable<T> : IList<T>
    {
        private readonly T[] array;

        public ArrayEnumerable(T[] array)
        {
            this.array = array ?? throw new ArgumentNullException(nameof(array));
        }

        public static implicit operator ArrayEnumerable<T>(T[] arr) => new ArrayEnumerable<T>(arr);

        public Enumerator GetEnumerator() => new Enumerator(array);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region IList<T> Support

        public void CopyTo(T[] arr, int arrayIndex)
        {
            array.CopyTo(arr, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public int Count => array.Length;
        public bool IsReadOnly => true;

        public int IndexOf(T item)
        {
            return Array.IndexOf(array, item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        #endregion

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] array;
            private int index;

            internal Enumerator(T[] array)
            {
                this.array = array;
                index = 0;
            }

            public bool MoveNext() => index++ < array.Length;

            public T Current => array[index - 1];

            object IEnumerator.Current => Current;

            public void Reset() => throw new NotSupportedException();

            public void Dispose()
            {
            }
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            foreach (var element in array)
            {
                if (comparer.Equals(element, item))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
