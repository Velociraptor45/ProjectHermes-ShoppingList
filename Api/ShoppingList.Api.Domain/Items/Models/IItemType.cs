﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public interface IItemType
{
    ItemTypeId Id { get; }
    ItemTypeName Name { get; }
    IReadOnlyCollection<IItemAvailability> Availabilities { get; }
    IItemType? Predecessor { get; }

    void SetPredecessor(IItemType predecessor);

    SectionId GetDefaultSectionIdForStore(StoreId storeId);

    bool IsAvailableAtStore(StoreId storeId);

    Task<IItemType> ModifyAsync(ItemTypeModification modification, IValidator validator);
    IItemType Update(StoreId storeId, Price price);
    IItemType Update();
}