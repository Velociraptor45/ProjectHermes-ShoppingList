﻿using Fluxor;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Effects;

public class NotificationEffects
{
    private readonly IShoppingListNotificationService _notificationService;

    public NotificationEffects(IShoppingListNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [EffectMethod]
    public Task HandleDisplayErrorNotificationAction(DisplayErrorNotificationAction action, IDispatcher dispatcher)
    {
        _notificationService.NotifyError(action.Title, action.Message);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleDisplayApiExceptionNotificationAction(DisplayApiExceptionNotificationAction action,
        IDispatcher dispatcher)
    {
        var contract = action.Exception.DeserializeContent<ErrorContract>();

        _notificationService.NotifyError(action.Title, contract.Message);
        return Task.CompletedTask;
    }
}