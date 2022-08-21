﻿using MediatR;
using Microservice.Banking.Core.Services;
using Microservice.Banking.Common;
using Microservice.Banking.Common.EventBus;
using Microservice.Banking.Core.IntegrationEvents;

namespace Microservice.Banking.Core.Commands
{
    public record Deposit : INotification
  {
    public Deposit(Guid accountId, Money amount)
    {
      AccountId = accountId;
      Amount = amount ?? throw new ArgumentNullException(nameof(amount));
    }

    public Guid AccountId { get; }
    public Money Amount { get; }
  }

  public class DepositHandler : INotificationHandler<Deposit>
  {
    private readonly IAggregateRepository<Account, Guid> _accountEventsService;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly IEventProducer _eventProducer;

    public DepositHandler(IAggregateRepository<Account, Guid> accountEventsService, ICurrencyConverter currencyConverter, IEventProducer eventProducer)
    {
      _accountEventsService = accountEventsService;
      _currencyConverter = currencyConverter;
      _eventProducer = eventProducer;
    }

    public async Task Handle(Deposit command, CancellationToken cancellationToken)
    {
      var account = await _accountEventsService.RehydrateAsync(command.AccountId);
      if (null == account)
        throw new ArgumentOutOfRangeException(nameof(Deposit.AccountId), "invalid account id");

      account.Deposit(command.Amount, _currencyConverter);

      await _accountEventsService.PersistAsync(account);

      var @event = new TransactionHappened(Guid.NewGuid(), account.Id);
      await _eventProducer.DispatchAsync(@event, cancellationToken);
    }
  }

}