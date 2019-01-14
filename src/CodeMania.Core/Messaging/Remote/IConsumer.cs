using System.Threading;
using System.Threading.Tasks;

namespace CodeMania.Core.Messaging.Remote
{
	public interface IConsumer
	{
		ValueTask ConsumeAsync(IConsumerContext context, CancellationToken cancellationToken);
	}
}