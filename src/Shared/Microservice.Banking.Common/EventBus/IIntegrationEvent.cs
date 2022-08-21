namespace Microservice.Banking.Common.EventBus;

public interface IIntegrationEvent
{
  Guid Id { get; }
}