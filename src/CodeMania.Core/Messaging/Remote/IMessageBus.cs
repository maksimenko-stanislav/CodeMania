using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging.Remote
{
	public interface IMessageBus : IPublisher, ISubscriber
	{
	}

	public abstract class MessageBusBase<TConsumerContext, TSubscription> : IMessageBus
		where TConsumerContext : ConsumerContextBase
		where TSubscription : SubscriptionBase<TConsumerContext>
	{
		private readonly Pool<TConsumerContext> consumerContextPool;
		private readonly ConcurrentDictionary<string, HashSet<TSubscription>> subscriptions;
		protected readonly CancellationTokenSource CancellationTokenSource;
		private readonly ReaderWriterLockSlim subscriptionReaderWriterLock;
		private readonly object syncRoot = new object();

		protected MessageBusBase([NotNull] CancellationTokenSource cancellationTokenSource)
		{
			CancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
			consumerContextPool = new Pool<TConsumerContext>(512, CreateConsumerContext);
			subscriptions = new ConcurrentDictionary<string, HashSet<TSubscription>>(StringComparer.Ordinal);
			subscriptionReaderWriterLock = new ReaderWriterLockSlim();
		}

		protected abstract TConsumerContext CreateConsumerContext();

		protected abstract TSubscription CreateSubscription(
			string subscription,
			IConsumer consumer,
			Pool<TConsumerContext> consumerContextPool,
			CancellationTokenSource cancellationTokenSource,
			string subscriptionGroup = null);

		public ValueTask PublishAsync([NotNull] string subscription, Payload payload, IDictionary<string, string> attributes,
			CancellationToken cancellationToken)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			return PublishAsyncCore(subscription, payload, attributes, cancellationToken);
		}

		protected abstract ValueTask PublishAsyncCore(string subscription, Payload payload,
			IDictionary<string, string> attributes,
			CancellationToken cancellationToken);

		public ValueTask<Payload> RequestAsync([NotNull] string subscription, Payload payload,
			IDictionary<string, string> attributes,
			CancellationToken cancellationToken)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			return RequestAsyncCore(subscription, payload, attributes, cancellationToken);
		}

		protected abstract ValueTask<Payload> RequestAsyncCore(string subscription, Payload payload,
			IDictionary<string, string> attributes,
			CancellationToken cancellationToken);

		public ISubscription Subscribe([NotNull] string subscription, [NotNull] IConsumer consumer,
			string subscriptionGroup = null)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			if (consumer == null)
			{
				throw new ArgumentNullException(nameof(consumer));
			}

			var result = CreateSubscription(subscription, consumer, consumerContextPool, CancellationTokenSource, subscriptionGroup);

			result.Disposed += SubscriptionBaseOnDisposed;

			var subscriptionsSet = subscriptions.GetOrAdd(subscription, s => new HashSet<TSubscription>());
			lock (syncRoot)
			{
				subscriptionsSet.Add(result);
			}

			return result;
		}

		private void SubscriptionBaseOnDisposed(object sender, EventArgs e)
		{
			if (sender is TSubscription subscription)
			{
				subscription.Disposed -= SubscriptionBaseOnDisposed;

				if (subscriptions.TryGetValue(subscription.Subscription, out var subscriptionSet))
				{
					lock (subscriptionSet)
					{
						subscriptionSet.Remove(subscription);
					}
				}
			}
		}
	}

	//internal sealed class ConcurrentHashSet<T> : IEnumerable<T>
	//{
	//	private readonly ConcurrentDictionary<T, byte> backend;

	//	public ConcurrentHashSet(IEqualityComparer<T> equalityComparer)
	//	{
	//		backend = new ConcurrentDictionary<T, byte>(equalityComparer ?? EqualityComparer<T>.Default);
	//	}

	//	public bool TryAdd(T item) => backend.TryAdd(item, 0);

	//	public bool Remove(T item) => backend.TryRemove(item, out _);

	//	public IEnumerator<T> GetEnumerator()
	//	{
	//		foreach (var pair in backend)
	//		{
	//			yield return pair.Key;
	//		}
	//	}

	//	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	//}
}