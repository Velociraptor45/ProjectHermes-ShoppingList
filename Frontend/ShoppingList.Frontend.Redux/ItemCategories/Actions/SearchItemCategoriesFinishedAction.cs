﻿using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);