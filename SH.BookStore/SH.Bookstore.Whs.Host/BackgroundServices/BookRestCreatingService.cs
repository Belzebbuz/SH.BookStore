using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Whs.Host.Contracts.Repositories;
using SH.Bookstore.Whs.Host.Mongo.Entites;

namespace SH.Bookstore.Whs.Host.BackgroundServices;
internal class BookRestCreatingService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IRepository<BookRest> _bookRestRepository;
    private readonly string _bookCreatedTopic;

    public BookRestCreatingService(
        IMessageSubscriber messageSubscriber, 
        IOptions<PulsarSettings> options,
        IRepository<BookRest> bookRestRepository)
    {
        _messageSubscriber = messageSubscriber;
        _bookRestRepository = bookRestRepository;
        _bookCreatedTopic = options.Value.BookCreatedTopic;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageSubscriber.SubscribeAsync<BookCreatedMessage>(_bookCreatedTopic, async message =>
        {
            await _bookRestRepository.CreateAsync(BookRest.Create(message.BookId));
        });
    }
}

public record BookCreatedMessage(Guid BookId) : IMessage;