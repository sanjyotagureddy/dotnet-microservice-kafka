using Confluent.Kafka;

namespace Microservice.Banking.Infrastructure.Kafka.Serializers;

internal class GuidDeserializer : IDeserializer<Guid>
{
  public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
  {
    return new Guid(data);
  }
}