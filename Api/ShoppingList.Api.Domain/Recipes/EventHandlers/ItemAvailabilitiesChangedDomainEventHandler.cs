﻿using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;

public class ItemAvailabilitiesChangedDomainEventHandler : IDomainEventHandler<ItemAvailabilitiesChangedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ILogger<ItemAvailabilityDeletedDomainEventHandler> _logger;

    public ItemAvailabilitiesChangedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ILogger<ItemAvailabilityDeletedDomainEventHandler> logger)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(ItemAvailabilitiesChangedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            () => "Started handling {EventName} for item '{ItemId}' for recipes",
            nameof(ItemAvailabilitiesChangedDomainEvent),
            domainEvent.ItemId);

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterAvailabilitiesChangedAsync(domainEvent.ItemId, domainEvent.ItemTypeId,
            domainEvent.NewAvailabilities);

        _logger.LogDebug(
            () => "Finished handling {EventName} for item '{ItemId}' for recipes",
            nameof(ItemAvailabilitiesChangedDomainEvent),
            domainEvent.ItemId);
    }
}