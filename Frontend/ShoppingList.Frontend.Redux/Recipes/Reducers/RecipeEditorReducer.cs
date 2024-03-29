﻿using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class RecipeEditorReducer
{
    [ReducerMethod(typeof(SetNewRecipeAction))]
    public static RecipeState OnSetNewRecipe(RecipeState state)
    {
        var recipe = new EditedRecipe(
            Guid.Empty,
            string.Empty,
            1,
            new List<EditedIngredient>
            {
                EditedIngredient.GetInitial(state.IngredientQuantityTypes)
            },
            new SortedSet<EditedPreparationStep>
            {
                new(Guid.NewGuid(), Guid.Empty, string.Empty, 0)
            },
            new List<Guid>(0));

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = recipe,
                IsInEditMode = true
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadRecipeForEditingFinished(RecipeState state, LoadRecipeForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Recipe = action.Recipe,
                IsInEditMode = false
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeNameChanged(RecipeState state, RecipeNameChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Name = action.Name
                }
            }
        };
    }

    [ReducerMethod(typeof(ToggleEditModeAction))]
    public static RecipeState OnToggleEditMode(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsInEditMode = !state.Editor.IsInEditMode
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeTagInputChanged(RecipeState state, RecipeTagInputChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                RecipeTagCreateInput = action.Input
            }
        };
    }

    [ReducerMethod(typeof(RecipeTagsDropdownClosedAction))]
    public static RecipeState OnRecipeTagsDropdownClosed(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                RecipeTagCreateInput = string.Empty
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeTagsChanged(RecipeState state, RecipeTagsChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    RecipeTagIds = action.RecipeTagIds
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnCreateNewRecipeTagFinished(RecipeState state, CreateNewRecipeTagFinishedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var allTags = state.RecipeTags.ToList();
        allTags.Add(action.NewTag);

        var recipeTagIds = state.Editor.Recipe.RecipeTagIds.ToList();
        recipeTagIds.Add(action.NewTag.Id);

        return state with
        {
            RecipeTags = allTags.OrderBy(t => t.Name).ToList(),
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    RecipeTagIds = recipeTagIds
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeNumberOfServingsChanged(RecipeState state,
        RecipeNumberOfServingsChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    NumberOfServings = action.NumberOfServings
                }
            }
        };
    }

    [ReducerMethod(typeof(ModifyRecipeStartedAction))]
    public static RecipeState OnModifyRecipeStarted(RecipeState state)
    {
        return SetSaving(state, true);
    }

    [ReducerMethod(typeof(ModifyRecipeFinishedAction))]
    public static RecipeState OnModifyRecipeFinished(RecipeState state)
    {
        return SetSaving(state, false);
    }

    [ReducerMethod(typeof(CreateRecipeStartedAction))]
    public static RecipeState OnCreateRecipeStarted(RecipeState state)
    {
        return SetSaving(state, true);
    }

    [ReducerMethod(typeof(CreateRecipeFinishedAction))]
    public static RecipeState OnCreateRecipeFinished(RecipeState state)
    {
        return SetSaving(state, false);
    }

    private static RecipeState SetSaving(RecipeState state, bool isSaving)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = isSaving
            }
        };
    }
}