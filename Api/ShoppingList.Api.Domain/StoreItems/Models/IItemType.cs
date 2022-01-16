﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IItemType
    {
        ItemTypeId Id { get; }
        string Name { get; }
        IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
        IItemType? Predecessor { get; }

        void SetPredecessor(IItemType predecessor);

        SectionId GetDefaultSectionIdForStore(StoreId storeId);

        bool IsAvailableAtStore(StoreId storeId);

        Task<IItemType> ModifyAsync(ItemTypeModification modification, IValidator validator);
    }
}