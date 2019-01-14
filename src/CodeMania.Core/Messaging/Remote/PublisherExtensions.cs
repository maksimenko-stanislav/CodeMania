using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging.Remote
{
	[PublicAPI]
	public static class PublisherExtensions
	{
		public static ValueTask PublishAsync(this IPublisher publisher, string subscription, Payload payload,
			CancellationToken cancellationToken) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.PublishAsync(subscription, payload, default, cancellationToken);

		public static ValueTask PublishAsync(this IPublisher publisher, string subscription, Payload payload) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.PublishAsync(subscription, payload, default, CancellationToken.None);

		public static ValueTask PublishAsync(this IPublisher publisher, string subscription, Payload payload,
			IDictionary<string, string> attributes) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.PublishAsync(subscription, payload, attributes, CancellationToken.None);

		public static ValueTask<Payload> RequestAsync(this IPublisher publisher, string subscription, Payload payload,
			CancellationToken cancellationToken) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.RequestAsync(subscription, payload, default, cancellationToken);

		public static ValueTask<Payload> RequestAsync(this IPublisher publisher, string subscription, Payload payload) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.RequestAsync(subscription, payload, default, CancellationToken.None);

		public static ValueTask<Payload> RequestAsync(this IPublisher publisher, string subscription, Payload payload,
			IDictionary<string, string> attributes) =>
			(publisher ?? throw new ArgumentNullException(nameof(publisher)))
			.RequestAsync(subscription, payload, attributes, CancellationToken.None);
	}
}