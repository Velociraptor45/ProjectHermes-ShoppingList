﻿using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists
{
    public class RemoveItemFromBasketRequest : IApiRequest
    {
        public RemoveItemFromBasketRequest(Guid requestId, Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
        public Guid? ItemTypeId { get; }
    }
}