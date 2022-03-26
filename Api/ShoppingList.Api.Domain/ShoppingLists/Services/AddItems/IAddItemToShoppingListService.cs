﻿using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public interface IAddItemToShoppingListService
{
    Task AddItemToShoppingListAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId, QuantityInBasket quantity,
        CancellationToken cancellationToken);

    Task AddItemToShoppingListAsync(IShoppingList shoppingList, TemporaryItemId temporaryItemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken);

    Task AddItemWithTypeToShoppingListAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken);

    Task AddItemWithTypeToShoppingList(IShoppingList shoppingList, IStoreItem item, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken);

    Task AddAsync(ShoppingListId shoppingListId, OfflineTolerantItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken);
}