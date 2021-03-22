﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemAvailabilityFactory
    {
        IStoreItemAvailability Create(IStoreItemStore store, float price, StoreItemSectionId defaultSectionId);

        IStoreItemAvailability Create(IStore store, float price, StoreItemSectionId defaultSectionId);

        IStoreItemAvailability Create(IStore store, float price, SectionId defaultSectionId);
    }
}