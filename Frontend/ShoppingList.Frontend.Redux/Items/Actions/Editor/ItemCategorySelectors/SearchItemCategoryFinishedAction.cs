﻿using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record SearchItemCategoryFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);