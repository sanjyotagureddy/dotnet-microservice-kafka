namespace Microservice.Banking.Core.Services
{
  public interface ICurrencyConverter
  {
    Money Convert(Money amount, Currency currency);
  }
}