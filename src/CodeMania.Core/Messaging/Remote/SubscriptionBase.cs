using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging.Remote
{
	public abstract class SubscriptionBase<TConsumerContext> : ISubscription
		where TConsumerContext : ConsumerContextBase
	{
		private readonly IConsumer consumer;
		private readonly Pool<TConsumerContext> consumerContextPool;
		private readonly CancellationTokenSource tokenSource;

		private volatile bool isDisposed;

		public event EventHandler Disposed;

		public string Subscription { get; }

		protected SubscriptionBase(
			[NotNull] string subscription,
			[NotNull] IConsumer consumer,
			[NotNull] Pool<TConsumerContext> consumerContextPool,
			[NotNull] CancellationTokenSource tokenSource)
		{
			Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
			this.consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
			this.consumerContextPool = consumerContextPool ?? throw new ArgumentNullException(nameof(consumerContextPool));
			this.tokenSource = tokenSource ?? throw new ArgumentNullException(nameof(tokenSource));
		}

		protected async ValueTask ConsumeAsync(Payload payload, IReadOnlyDictionary<string, string> attributes)
		{
			var consumerContext = consumerContextPool.Rent();
			try
			{
				consumerContext.Initialize(payload, attributes);

				await consumer.ConsumeAsync(consumerContext, tokenSource.Token);
			}
			finally
			{
				consumerContext.Reset();
				consumerContextPool.Return(consumerContext);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed) return;
			isDisposed = true;
			Disposed?.Invoke(this, EventArgs.Empty);
		}

		~SubscriptionBase()
		{
			Dispose(false);
		}
	}
}