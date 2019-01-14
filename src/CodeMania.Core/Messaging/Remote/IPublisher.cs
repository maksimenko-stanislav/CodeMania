using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMania.Core.Messaging.Remote
{
	public interface IPublisher
	{
		ValueTask PublishAsync(string subscription, Payload payload, IDictionary<string, string> attributes, CancellationToken cancellationToken);

		ValueTask<Payload> RequestAsync(string subscription, Payload payload, IDictionary<string, string> attributes, CancellationToken cancellationToken);
	}

	public interface ITypedPublisher
	{
		ValueTask PublishAsync<T>(string subscription, T payload, IDictionary<string, string> attributes, CancellationToken cancellationToken);

		ValueTask<TResponse> RequestAsync<TRequest, TResponse>(string subscription, TRequest request, IDictionary<string, string> attributes, CancellationToken cancellationToken);
	}
}