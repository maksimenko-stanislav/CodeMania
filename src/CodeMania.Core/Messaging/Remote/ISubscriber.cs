namespace CodeMania.Core.Messaging.Remote
{
	public interface ISubscriber
	{
		ISubscription Subscribe(string subscription, IConsumer consumer, string subscriptionGroup = null);
	}
}