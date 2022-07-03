﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public class ItemAvailabilityFactory : IItemAvailabilityFactory
{
    public IItemAvailability Create(StoreId storeId, Price price, SectionId defaultSectionId)
    {
        return new ItemAvailability(storeId, price, defaultSectionId);
    }
}