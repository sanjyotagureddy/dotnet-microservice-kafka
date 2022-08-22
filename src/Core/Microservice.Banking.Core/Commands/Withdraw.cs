using MediatR;
using Microservice.Banking.Core.Services;
using Microservice.Banking.Common;
using Microservice.Banking.Common.EventBus;
using Microservice.Banking.Core.IntegrationEvents;

namespace Microservice.Banking.Core.Commands;

public record Withdraw : INotification
{
  public Withdraw(Guid accountId, Money amount)
  {
    AccountId = accountId;
    Amount = amount ?? throw new ArgumentNullException(nameof(amount));
  }

  public Guid AccountId { get; }
  public Money Amount { get; }
}


public class WithdrawHandler : INotificationHandler<Withdraw>
{
  private readonly IAggregateRepository<Account, Guid> _accountEventsService;
  private readonly ICurrencyConverter _currencyConverter;
  private readonly IEventProducer _eventProducer;

  public WithdrawHandler(IAggregateRepository<Account, Guid> accountEventsService, ICurrencyConverter currencyConverter, IEventProducer eventProducer)
  {
    _accountEventsService = accountEventsService;
    _currencyConverter = currencyConverter;
    _eventProducer = eventProducer;
  }

  public async Task Handle(Withdraw command, CancellationToken cancellationToken)
  {
    var account = await _accountEventsService.RehydrateAsync(command.AccountId, cancellationToken);
    if (null == account)
      throw new ArgumentOutOfRangeException(nameof(Withdraw.AccountId), "invalid account id");

    account.Withdraw(command.Amount, _currencyConverter);

    await _accountEventsService.PersistAsync(account, cancellationToken);

    var @event = new TransactionHappened(Guid.NewGuid(), account.Id);
    await _eventProducer.DispatchAsync(@event, cancellationToken: cancellationToken);
  }
}