using Microservice.Banking.Common.Models;

namespace Microservice.Banking.Common;

public interface IAggregateRepository<TA, TKey>
  where TA : class, IAggregateRoot<TKey>
{
  Task PersistAsync(TA aggregateRoot, CancellationToken cancellationToken = default);
  Task<TA> RehydrateAsync(TKey key, CancellationToken cancellationToken = default);
}