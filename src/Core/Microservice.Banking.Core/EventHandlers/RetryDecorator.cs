﻿using MediatR;
using Polly;

namespace Microservice.Banking.Core.EventHandlers;

public class RetryDecorator<TNotification> : MediatR.INotificationHandler<TNotification>
  where TNotification : MediatR.INotification
{
  private readonly INotificationHandler<TNotification> _inner;
  private readonly Polly.IAsyncPolicy _retryPolicy;

  public RetryDecorator(MediatR.INotificationHandler<TNotification> inner)
  {
    _inner = inner; //TODO: check RetryDecorator doesn't get injected twice
    _retryPolicy = Polly.Policy.Handle<ArgumentOutOfRangeException>()
      .WaitAndRetryAsync(3,
        i => TimeSpan.FromSeconds(i));
  }

  public Task Handle(TNotification notification, CancellationToken cancellationToken)
  {
    return _retryPolicy.ExecuteAsync(() => _inner.Handle(notification, cancellationToken));
  }
}