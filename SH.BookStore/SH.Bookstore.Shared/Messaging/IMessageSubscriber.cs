namespace SH.Bookstore.Shared.Messaging;

public interface IMessageSubscriber
{
    public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class, IMessage;
}