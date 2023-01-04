using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SH.Bookstore.Shared.Messaging.Pulsar;
public static class Extensions
{
    public static IServiceCollection AddPulsarMessaging(this IServiceCollection services, IConfiguration config)
        => services
        .Configure<PulsarSettings>(config.GetSection(nameof(PulsarSettings)))
        .AddSingleton<IMessagePublisher, PulsarMessagePublisher>()
        .AddSingleton<IMessageSubscriber, PulsarMessageSubscriber>();
}
