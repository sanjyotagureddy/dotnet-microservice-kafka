using Microservice.Banking.Core.Services;

namespace Microservice.Banking.Core;

public record Money
{
  public Money(Currency currency, decimal value)
  {
    Value = value;
    Currency = currency ?? throw new ArgumentNullException(nameof(currency));
  }

  public decimal Value { get; }
  public Currency Currency { get; }

  public Money Subtract(Money other, ICurrencyConverter? converter = null)
  {
    if (other is null)
      throw new ArgumentNullException(nameof(other));

    if (other.Currency != Currency)
    {
      if (converter is null)
        throw new ArgumentNullException(nameof(converter), "Currency Converter is requried when currencies don't match");

      var converted = converter.Convert(other, Currency);
      return new Money(Currency, Value - converted.Value);
    }

    return new Money(Currency, Value - other.Value);
  }

  public Money Add(Money other, ICurrencyConverter? converter = null)
  {
    if (other is null)
      throw new ArgumentNullException(nameof(other));

    if (other.Currency != Currency)
    {
      if (converter is null)
        throw new ArgumentNullException(nameof(converter), "Currency Converter is requried when currencies don't match");

      var converted = converter.Convert(other, Currency);
      return new Money(Currency, Value + converted.Value);
    }

    return new Money(Currency, Value + other.Value);
  }

  public override string ToString() => $"{Value} {Currency}";

  public static Money Zero(Currency currency) => new Money(currency, 0);
}