﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListItemFactory : IShoppingListItemFactory
    {
        public IShoppingListItem Create(ItemId id, bool isInBasket, float quantity)
        {
            return new ShoppingListItem(
                id,
                isInBasket,
                quantity);
        }
    }
}