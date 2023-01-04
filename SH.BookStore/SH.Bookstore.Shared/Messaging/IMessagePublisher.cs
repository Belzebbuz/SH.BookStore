namespace SH.Bookstore.Shared.Messaging;

public interface IMessagePublisher
{
    public Task PublishAsync<T>(string topic, T message) where T : class, IMessage; 
}
