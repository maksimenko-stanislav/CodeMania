using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMania.Core.Messaging.Remote
{
	public interface IConsumerContext : IPublisher
	{
		Payload Payload { get; }
		IReadOnlyDictionary<string, string> Attributes { get; }
		ValueTask RespondAsync(Payload payload, IDictionary<string, string> attributes, CancellationToken cancellationToken);
	}
}