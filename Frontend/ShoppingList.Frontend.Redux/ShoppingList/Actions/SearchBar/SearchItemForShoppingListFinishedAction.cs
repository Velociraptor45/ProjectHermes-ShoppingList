﻿using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
public record SearchItemForShoppingListFinishedAction(IEnumerable<SearchItemForShoppingListResult> Results);