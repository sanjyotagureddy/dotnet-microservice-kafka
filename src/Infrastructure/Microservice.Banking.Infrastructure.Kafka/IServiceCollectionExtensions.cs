using Microservice.Banking.Common.EventBus;
using Microservice.Banking.Common.Models;
using Microservice.Banking.Infrastructure.Kafka.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microservice.Banking.Infrastructure.Kafka;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddKafkaEventProducer<TA, TK>(this IServiceCollection services, string topicName, KafkaConfig configuration)
    where TA : class, IAggregateRoot<TK>
  {
    return services.AddSingleton<IEventProducer>(ctx =>
    {
      var logger = ctx.GetRequiredService<ILogger<EventProducer>>();
      return new EventProducer(logger, topicName, configuration);
    });
  }
}