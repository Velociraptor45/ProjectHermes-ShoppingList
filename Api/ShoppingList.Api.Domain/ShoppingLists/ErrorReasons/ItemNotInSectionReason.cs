﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;

public class ItemNotInSectionReason : IReason
{
    public ItemNotInSectionReason(ItemId shoppingListItemId, SectionId sectionId)
    {
        Message = $"Item {shoppingListItemId} isn't in section {sectionId}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotInSection;
}