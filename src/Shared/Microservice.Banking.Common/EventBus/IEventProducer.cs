namespace Microservice.Banking.Common.EventBus;

public interface IEventProducer
{
  Task DispatchAsync(IIntegrationEvent @event, IDictionary<string, string>? customHeaders = null, CancellationToken cancellationToken = default);
}