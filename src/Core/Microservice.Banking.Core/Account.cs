using Microservice.Banking.Common.Models;
using Microservice.Banking.Core.DomainEvents;
using Microservice.Banking.Core.Services;

namespace Microservice.Banking.Core;

public record Account : BaseAggregateRoot<Account, Guid>
{
  private Account() { }

  public Account(Guid id, Customer owner, Currency currency) : base(id)
  {
    if (owner == null)
      throw new ArgumentNullException(nameof(owner));
    if (currency == null)
      throw new ArgumentNullException(nameof(currency));

    this.Append(new AccountEvents.AccountCreated(this, owner, currency));
  }

  public Guid OwnerId { get; private set; }
  public Money Balance { get; private set; }

  public void Withdraw(Money amount, ICurrencyConverter currencyConverter)
  {
    if (amount.Value < 0)
      throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

    var normalizedAmount = currencyConverter.Convert(amount, Balance.Currency);
    if (normalizedAmount.Value > Balance.Value)
      throw new AccountTransactionException($"unable to withdrawn {normalizedAmount} from account {this.Id}", this);

    this.Append(new AccountEvents.Withdrawal(this, amount));
  }

  public void Deposit(Money amount, ICurrencyConverter currencyConverter)
  {
    if (amount.Value < 0)
      throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

    var normalizedAmount = currencyConverter.Convert(amount, Balance.Currency);

    this.Append(new AccountEvents.Deposit(this, normalizedAmount));
  }

  protected override void When(IDomainEvent<Guid> @event)
  {
    switch (@event)
    {
      case AccountEvents.AccountCreated c:
        this.Id = c.AggregateId;
        Balance = Money.Zero(c.Currency);
        OwnerId = c.OwnerId;
        break;
      case AccountEvents.Withdrawal w:
        Balance = Balance.Subtract(w.Amount);
        break;
      case AccountEvents.Deposit d:
        Balance = Balance.Add(d.Amount);
        break;
    }
  }

  public static Account Create(Guid accountId, Customer owner, Currency currency)
  {
    var account = new Account(accountId, owner, currency);
    owner.AddAccount(account);
    return account;
  }
}