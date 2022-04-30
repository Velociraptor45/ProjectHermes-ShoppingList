﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ItemAtStoreNotAvailableReason : IReason
{
    public ItemAtStoreNotAvailableReason(ItemId itemId, StoreId storeId)
    {
        Message = $"Item {itemId.Value} not available at store {storeId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
}