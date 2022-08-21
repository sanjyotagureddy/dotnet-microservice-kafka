using Microservice.Banking.Common.Models;

namespace Microservice.Banking.Common;

public interface IEventSerializer
{
  IDomainEvent<TKey>? Deserialize<TKey>(string type, byte[] data);
  IDomainEvent<TKey>? Deserialize<TKey>(string type, string data);
  byte[] Serialize<TKey>(IDomainEvent<TKey> @event);
}