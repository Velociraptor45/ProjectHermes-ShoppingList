﻿using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
public record LoadAddToShoppingListFinishedAction(IReadOnlyCollection<AddToShoppingListItem> ItemsForOneServing);