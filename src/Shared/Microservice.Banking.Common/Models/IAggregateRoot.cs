namespace Microservice.Banking.Common.Models;

public interface IAggregateRoot<out TKey> : IEntity<TKey>
{
  long Version { get; }
  IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
  void ClearEvents();
}