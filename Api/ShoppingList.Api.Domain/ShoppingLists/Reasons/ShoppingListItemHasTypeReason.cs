﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListItemHasTypeReason : IReason
{
    public ShoppingListItemHasTypeReason(ShoppingListId shoppingListId, ItemId itemId)
    {
        Message = $"Item {itemId.Value} on shopping list {shoppingListId.Value} has types.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListItemHasType;
}