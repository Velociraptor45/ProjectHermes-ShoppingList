﻿namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;

public record AddIngredientToShoppingListChangedAction(Guid IngredientKey, bool AddToShoppingList);