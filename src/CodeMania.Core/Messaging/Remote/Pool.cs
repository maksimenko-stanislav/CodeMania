using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging.Remote
{
	public sealed class Pool<T>
	{
		private readonly struct Element
		{
			private readonly T data;

			public Element(T data)
			{
				this.data = data;
			}

			public static implicit operator T(Element element) => element.data;

			public static implicit operator Element(T data) => new Element(data);
		}

		private readonly int instancesToCreate;
		private readonly Func<T> factory;
		private readonly ConcurrentQueue<Element> pool;
		private int count;

		public Pool(int instancesToCreate, [NotNull] Func<T> factory)
		{
			if (instancesToCreate < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(instancesToCreate));
			}

			this.instancesToCreate = instancesToCreate;
			this.factory = factory ?? throw new ArgumentNullException(nameof(factory));

			pool = new ConcurrentQueue<Element>(Enumerable.Range(0, instancesToCreate)
				.Select(x => new Element(factory())));
		}

		public T Rent()
		{
			if (pool.TryDequeue(out var element))
			{
				Interlocked.Decrement(ref count);

				return element;
			}

			return factory();
		}

		public bool Return(T data)
		{
			if (count > instancesToCreate) return false;

			pool.Enqueue(data);

			Interlocked.Increment(ref count);

			return true;
		}
	}
}