#pragma warning disable CS8618
namespace Microservice.Banking.Infrastructure.Kafka.Configs;

/// <summary>
/// Main configuration object for Kafka settings.
/// </summary>
public class KafkaConfig
{
  /// <summary>
  /// Kafka message producer configuration.
  /// </summary>
  public KafkaProducerConfig Producer { get; set; }

  /// <summary>
  /// Kafka message consumers configurations.
  /// </summary>
  public List<KafkaConsumerConfig> Consumers { get; set; } = new List<KafkaConsumerConfig>();

  /// <summary>
  /// Kafka schema registry configuration.
  /// </summary>
  public KafkaSchemaRegistryConfig SchemaRegistry { get; set; }
}

/// <summary>
/// Configuration object for Kafka producer settings.
/// </summary>
public class KafkaProducerConfig
{
  /// <summary>
  /// Set of key values with settings for Kafka producer. This can contain any of Kafka built in settings.
  /// </summary>
  public IDictionary<string, string> Settings { get; set; }

  /// <summary>
  /// Enables automatic schema registrations into registry for unrecognized schemas. By default set to false.
  /// </summary>
  public bool AutoRegisterSchemas { get; set; } = false;

  /// <summary>
  /// Sets whether Kafka producer should check message compatibility with Avro Schemas. By default set to true.
  /// </summary>
  public bool UseAvro { get; set; } = true;
}

/// <summary>
/// Configuration object for Kafka consumer settings.
/// </summary>
public class KafkaConsumerConfig
{
  /// <summary>
  /// Name for this consumer configuration which will be used to associate it with consumer registration.
  /// </summary>
  public string ConsumerName { get; set; }

  /// <summary>
  /// Set of key values with settings for Kafka consumer. This can contain any of Kafka built in settings.
  /// </summary>
  public IDictionary<string, string> Settings { get; set; }

  /// <summary>
  /// List of comma delimited topics to consume from.
  /// </summary>
  public string ConsumerTopics { get; set; }

  /// <summary>
  /// Sets whether Kafka consumer should check message compatibility with Avro Schemas. By default set to true.
  /// </summary>
  public bool UseAvro { get; set; } = true;
}

/// <summary>
/// Configuration object for Confluent Kafka Schema Registry settings.
/// </summary>
public class KafkaSchemaRegistryConfig
{
  /// <summary>
  /// Set of key values with settings for the schema registry. This can contain any of Kafka built in settings. 
  /// </summary>
  public IDictionary<string, string> Settings { get; set; }
}