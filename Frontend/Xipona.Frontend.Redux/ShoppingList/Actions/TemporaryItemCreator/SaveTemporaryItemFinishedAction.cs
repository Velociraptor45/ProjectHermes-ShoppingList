﻿using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
public record SaveTemporaryItemFinishedAction(ShoppingListItem Item, ShoppingListStoreSection Section);