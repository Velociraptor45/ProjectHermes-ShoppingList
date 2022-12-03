﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Recipe : IRecipe
{
    private readonly Ingredients _ingredients;
    private readonly PreparationSteps _steps;

    public Recipe(RecipeId id, RecipeName name, Ingredients ingredients, PreparationSteps steps)
    {
        _ingredients = ingredients;
        _steps = steps;
        Id = id;
        Name = name;
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; private set; }
    public IReadOnlyCollection<IIngredient> Ingredients => _ingredients.AsReadOnly();
    public IReadOnlyCollection<IPreparationStep> PreparationSteps => _steps.AsReadOnly();

    public async Task ModifyAsync(RecipeModification modification, IValidator validator)
    {
        Name = modification.Name;
        await _ingredients.ModifyManyAsync(modification.IngredientModifications, validator);
        _steps.ModifyMany(modification.PreparationStepModifications);
    }

    public void RemoveDefaultItem(ItemId defaultItemId)
    {
        _ingredients.RemoveDefaultItem(defaultItemId);
    }
}