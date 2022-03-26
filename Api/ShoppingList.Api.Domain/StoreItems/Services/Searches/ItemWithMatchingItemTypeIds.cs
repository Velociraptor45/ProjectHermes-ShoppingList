﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;

public class ItemWithMatchingItemTypeIds
{
    public ItemWithMatchingItemTypeIds(IStoreItem item, IEnumerable<ItemTypeId> matchingItemTypeIds)
    {
        Item = item;
        MatchingItemTypeIds = matchingItemTypeIds.ToList();
    }

    public IStoreItem Item { get; }
    public IReadOnlyCollection<ItemTypeId> MatchingItemTypeIds { get; }
}