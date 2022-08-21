namespace Microservice.Banking.Common.Models;

public interface IEntity<out TKey>
{
  TKey Id { get; }
}