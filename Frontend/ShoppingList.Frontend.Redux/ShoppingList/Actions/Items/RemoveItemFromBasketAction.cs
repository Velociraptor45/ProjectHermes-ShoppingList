﻿using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);