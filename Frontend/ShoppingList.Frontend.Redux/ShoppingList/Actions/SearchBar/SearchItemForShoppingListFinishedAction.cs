﻿using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
public record SearchItemForShoppingListFinishedAction(IEnumerable<SearchItemForShoppingListResult> Results);