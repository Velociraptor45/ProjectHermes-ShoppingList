﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

public interface IAvailabilityValidationService
{
    Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities, CancellationToken cancellationToken);
}