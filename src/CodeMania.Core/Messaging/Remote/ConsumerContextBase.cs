using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging.Remote
{
	public abstract class ConsumerContextBase : IConsumerContext
	{
		private readonly IPublisher publisher;
		private static readonly Payload EmptyPayload = Array.Empty<byte>();

		private Payload currentPayload;
		private IReadOnlyDictionary<string, string> currentAttributes;

		protected ConsumerContextBase([NotNull] IPublisher publisher)
		{
			this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
		}

		public ValueTask PublishAsync([NotNull] string subscription, Payload payload, IDictionary<string, string> attributes,
			CancellationToken cancellationToken)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			return publisher.PublishAsync(subscription, payload, attributes, cancellationToken);
		}

		public async ValueTask<Payload> RequestAsync([NotNull] string subscription, Payload payload, IDictionary<string, string> attributes,
			CancellationToken cancellationToken)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			return await publisher.RequestAsync(subscription, payload, attributes, cancellationToken);
		}

		internal void Initialize(Payload payload, IReadOnlyDictionary<string, string> attributes)
		{
			currentPayload = payload;
			currentAttributes = attributes;
		}

		internal void Reset()
		{
			currentPayload = EmptyPayload;
		}

		public Payload Payload => currentPayload;

		public IReadOnlyDictionary<string, string> Attributes => currentAttributes;

		public ValueTask RespondAsync(Payload payload, IDictionary<string, string> attributes, CancellationToken cancellationToken)
		{
			return RespondAsyncCore(payload, attributes, cancellationToken);
		}

		protected abstract ValueTask RespondAsyncCore(Payload payload, IDictionary<string, string> attributes,
			CancellationToken cancellationToken);
	}
}