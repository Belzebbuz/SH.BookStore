using DotPulsar.Abstractions;
using DotPulsar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Collections.Concurrent;
using System.Reflection;
using DotPulsar.Extensions;
using SH.Bookstore.Shared.Common.Services.Serialize;
using Microsoft.Extensions.Options;

namespace SH.Bookstore.Shared.Messaging.Pulsar;
internal class PulsarMessagePublisher : IMessagePublisher
{
    private readonly ConcurrentDictionary<string, IProducer<ReadOnlySequence<byte>>> _producers = new();
    private readonly ISerializerService _serializer;
    private readonly ILogger<PulsarMessagePublisher> _logger;
    private readonly IPulsarClient _client;
    private readonly string _producerName;

    public PulsarMessagePublisher(ISerializerService serializer,
        ILogger<PulsarMessagePublisher> logger, IOptions<PulsarSettings> options)
    {
        _serializer = serializer;
        _logger = logger;
        _client = PulsarClient.Builder().ServiceUrl(new Uri(options.Value.SeriviceUrl)).Build();
        _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
    }

    public async Task PublishAsync<T>(string topic, T message) where T : class, IMessage
    {
        var producer = _producers.GetOrAdd(topic, _client.NewProducer()
            .ProducerName(_producerName)
            .Topic($"persistent://public/default/{topic}")
            .Create());

        var payload = _serializer.SerializeBytes(message);
        var metadata = new MessageMetadata
        {
            ["custom_id"] = Guid.NewGuid().ToString("N"),
            ["producer"] = _producerName,
        };
        var messageId = await producer.Send(metadata, payload);
        _logger.LogInformation($"Sent a message with ID: '{messageId}'");
    }
}
