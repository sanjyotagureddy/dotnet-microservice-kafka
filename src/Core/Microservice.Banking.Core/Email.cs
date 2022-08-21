namespace Microservice.Banking.Core
{
  public record Email
  {
    public Email(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentNullException(nameof(value));
      if (!value.Contains('@'))
        throw new ArgumentException($"invalid email address: '{value}'", nameof(value));
      Value = value;
    }
    public string Value { get; }

    public override string ToString()
        => Value;
  }
}