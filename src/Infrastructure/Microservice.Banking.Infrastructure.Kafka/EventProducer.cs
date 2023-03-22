using System.Text;
using Confluent.Kafka;
using Microservice.Banking.Common.EventBus;
using Microservice.Banking.Infrastructure.Kafka.Configs;
using Microservice.Banking.Infrastructure.Kafka.Serializers;
using Microsoft.Extensions.Logging;

namespace Microservice.Banking.Infrastructure.Kafka;

public class EventProducer : IEventProducer, IDisposable
{
  private readonly IProducer<Guid, string> _producer;
  private readonly ILogger<EventProducer> _logger;
  private readonly string _topicName;
  private bool _isDisposed;

  public EventProducer(ILogger<EventProducer> logger, string topicName, KafkaConfig kafkaConfig)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
    var kafkaConfig1 = kafkaConfig ?? throw new ArgumentNullException(nameof(kafkaConfig));

    var producerConfig = new ProducerConfig(kafkaConfig1.Producer.Settings);
    _producer = new ProducerBuilder<Guid, string>(producerConfig).SetKeySerializer(new KeySerializer<Guid>())
      .Build();
  }

  public async Task DispatchAsync(IIntegrationEvent @event, IDictionary<string, string>? customHeaders = null, CancellationToken cancellationToken = default)
  {
    if (null == @event)
      throw new ArgumentNullException(nameof(@event));

    _logger.LogInformation("publishing event {EventId} ...", @event.Id);
    var eventType = @event.GetType();

    var serialized = System.Text.Json.JsonSerializer.Serialize(@event, eventType);
    var headers = new Headers
    {
      {"id", Encoding.UTF8.GetBytes(@event.Id.ToString())},
      {"type", Encoding.UTF8.GetBytes(@event.GetType().AssemblyQualifiedName!)}
    };
    if (customHeaders is { })
    {
      foreach (var (key, value) in customHeaders)
      {
        headers.Add(key, Encoding.UTF8.GetBytes(value));
      }
    }

    var message = new Message<Guid, string>()
    {
      Key = @event.Id,
      Value = serialized,
      Headers = headers
    };

    await _producer.ProduceAsync(_topicName, message, cancellationToken);
  }

  /// <summary>
  /// Called on object dispose.
  /// </summary>
  /// <param name="disposing">true if called by Dispose()</param>
  protected virtual void Dispose(bool disposing)
  {
    if (_isDisposed) return;
    if (disposing)
    {
      _producer?.Dispose();
    }

    _isDisposed = true;
  }

  /// <summary>
  /// Disposes current object.
  /// </summary>
  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}
