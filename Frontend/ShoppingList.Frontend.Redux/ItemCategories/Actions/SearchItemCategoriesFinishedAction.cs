﻿using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ShoppingList.Frontend.Redux.ItemCategories.Actions;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);