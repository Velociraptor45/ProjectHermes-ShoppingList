﻿using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class ItemCategorySelectorReducer
{
    [ReducerMethod]
    public static RecipeState OnItemCategoryInputChanged(RecipeState state, ItemCategoryInputChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with
        {
            ItemCategorySelector = action.Ingredient.ItemCategorySelector with
            {
                Input = action.Input
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnItemCategoryDropdownClosed(RecipeState state, ItemCategoryDropdownClosedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with
        {
            ItemCategorySelector = action.Ingredient.ItemCategorySelector with
            {
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSelectedItemCategoryChanged(RecipeState state, SelectedItemCategoryChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;
        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategoryId = action.ItemCategoryId,
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = ingredient.ItemCategorySelector.ItemCategories
                    .Where(i => i.Id == action.ItemCategoryId)
                    .ToList(),
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadInitialItemCategoryFinished(RecipeState state,
        LoadInitialItemCategoryFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = new List<ItemCategorySearchResult> { action.Result }
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSearchItemCategoriesFinished(RecipeState state, SearchItemCategoriesFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        var results = action.ItemCategories
            .OrderBy(r => r.Name)
            .ToList();

        var currentlySelected = ingredient.ItemCategorySelector.ItemCategories
            .FirstOrDefault(cat => cat.Id == ingredient.ItemCategoryId);

        if (currentlySelected != null && results.All(r => r.Id != currentlySelected.Id))
            results.Insert(0, currentlySelected);

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = results
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnCreateNewItemCategoryFinished(RecipeState state,
        CreateNewItemCategoryFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategoryId = action.SearchResult.Id,
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = new List<ItemCategorySearchResult> { action.SearchResult },
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }
}