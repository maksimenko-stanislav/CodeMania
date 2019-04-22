using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace CodeMania.Core.Utils
{
	public sealed class ReusableObjectPool<T>
	{
		private readonly int maxSize;
		private readonly Func<T> itemFactory;
		private readonly ConcurrentQueue<Element> pool;
		private int count;

		public ReusableObjectPool(int maxSize, Func<T> itemFactory)
		{
			if (maxSize < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxSize), "Value must be greater than zero.");
			}

			this.maxSize = maxSize;
			this.itemFactory = itemFactory ?? throw new ArgumentNullException(nameof(itemFactory));
			pool = new ConcurrentQueue<Element>(Enumerable.Range(0, maxSize).Select(x => new Element(itemFactory())));
		}

		public T Rent()
		{
			Element element;
			if (pool.TryDequeue(out element))
			{
				Interlocked.Decrement(ref count);
				return element;
			}

			return itemFactory();
		}

		public void Return(T item)
		{
			if (count < maxSize)
			{
				pool.Enqueue(item);
				Interlocked.Increment(ref count);
			}
		}

		private struct Element
		{
			private readonly T value;

			public Element(T value)
			{
				this.value = value;
			}

			public static implicit operator T(Element element) => element.value;
			public static implicit operator Element(T value) => new Element(value);
		}
	}
}