using MediatR;
using Microservice.Banking.Common.EventBus;

namespace Microservice.Banking.Core.IntegrationEvents;

public record TransactionHappened : IIntegrationEvent, INotification
{
  public TransactionHappened(Guid id, Guid accountId)
  {
    Id = id;
    AccountId = accountId;
  }

  public Guid AccountId { get; init; }
  public Guid Id { get; }
}