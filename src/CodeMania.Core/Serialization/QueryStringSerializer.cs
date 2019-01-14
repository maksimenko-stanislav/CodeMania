using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization
{
	public abstract class QueryStringSerializerBase
	{
		internal static readonly SerializationContextPool SerializationContexts = new SerializationContextPool(512);

		#region Nested Types

		public sealed class SerializationContext
		{
			public readonly StringBuilder Builder;
			public readonly QueryStringWriter Writer;

			public SerializationContext()
			{
				Builder = new StringBuilder(2096);
				Writer = new QueryStringWriter(Builder);
			}
		}

		public sealed class SerializationContextPool
		{
			private readonly int maxSize;
			private readonly ConcurrentQueue<SerializationContext> pool;
			private int count;

			public SerializationContextPool(int maxSize)
			{
				this.maxSize = maxSize;
				pool = new ConcurrentQueue<SerializationContext>(Enumerable.Range(0, maxSize)
					.Select(x => new SerializationContext()));
			}

			public SerializationContext Rent()
			{
				SerializationContext context;
				if (pool.TryDequeue(out context))
				{
					Interlocked.Decrement(ref count);
					return context;
				}

				return new SerializationContext();
			}

			public void Return(SerializationContext context)
			{
				if (count < maxSize)
				{
					pool.Enqueue(context);
					Interlocked.Increment(ref count);
				}
			}
		}

		#endregion
	}

	public class QueryStringSerializer<T> : QueryStringSerializerBase
	{
		private static readonly Lazy<QueryStringSerializer<T>> LazyGetDefaultInstance =
			new Lazy<QueryStringSerializer<T>>(() => QueryStringSerializerBuilder.Create<T>().Build());

		private static readonly Lazy<QueryStringSerializer<T>> LazyGetDataContractBasedInstance =
			new Lazy<QueryStringSerializer<T>>(() =>
				QueryStringSerializerBuilder.Create<T>().UseDataContractAttributesForNames().Build());

		public static QueryStringSerializer<T> Default => LazyGetDefaultInstance.Value;

		public static QueryStringSerializer<T> DataContractBased => LazyGetDataContractBasedInstance.Value;

		private readonly Action<T, QueryStringWriter> serializer;

		internal QueryStringSerializer(IEnumerable<PropertyContext<T>> propertyContexts)
		{
			if (propertyContexts == null)
				throw new ArgumentNullException(nameof(propertyContexts));

			serializer = GetSerializer(propertyContexts.OrderBy(x => x.SerializationName));
		}

		private Action<T, QueryStringWriter> GetSerializer(IEnumerable<PropertyContext<T>> propertyContexts)
		{
			var objParameter = Expression.Parameter(typeof(T), "obj");
			var writerParameter = Expression.Parameter(typeof(QueryStringWriter), "writer");

			// generates expression to call serializer for each property
			var result = Expression.Lambda<Action<T, QueryStringWriter>>(
				// call all serializers
				Expression.Block(
					// result type
					typeof(void),
					// variables (empty because we pass arguments directly to property serializers)
					Enumerable.Empty<ParameterExpression>(),
					// invoke all serializers
					propertyContexts.Select(x => x.GetSerializerExpression(objParameter, writerParameter))
				),
				// parameters
				objParameter,
				writerParameter);

			return ExpressionCompiler.Default.Compile(result);
		}

		public string Serialize(T obj)
		{
			var context = SerializationContexts.Rent();
			try
			{
				Serialize(obj, context.Writer);

				return context.Builder.ToString(0, context.Builder.Length - 1);
			}
			finally
			{
				context.Writer.Reset();
				SerializationContexts.Return(context);
			}
		}

		public string Serialize(T obj, [NotNull] StringBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			if (obj == null) return builder.ToString();

			if (builder.Length > 0 && builder[builder.Length - 1] != '&')
			{
				builder.Append('&');
			}

			using (var writer = new QueryStringWriter(builder))
			{
				Serialize(obj, writer);
			}

			return builder.ToString(0, builder.Length - 1);
		}

		private void Serialize(T obj, QueryStringWriter writer)
		{
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			if (obj != null)
			{
				serializer(obj, writer);
			}
		}
	}
}