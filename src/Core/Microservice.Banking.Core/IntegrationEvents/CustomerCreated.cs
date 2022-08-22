using MediatR;
using Microservice.Banking.Common.EventBus;

namespace Microservice.Banking.Core.IntegrationEvents;

public record CustomerCreated : IIntegrationEvent, INotification
{
  public CustomerCreated(Guid id, Guid customerId)
  {
    Id = id;
    CustomerId = customerId;
  }

  public Guid Id { get; }
  public Guid CustomerId { get; }
}