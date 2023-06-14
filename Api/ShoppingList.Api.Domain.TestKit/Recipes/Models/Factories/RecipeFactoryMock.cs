﻿using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;

public class RecipeFactoryMock : Mock<IRecipeFactory>
{
    public RecipeFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateNewAsync(RecipeCreation creation, IRecipe returnValue)
    {
        Setup(m => m.CreateNewAsync(creation))
            .ReturnsAsync(returnValue);
    }

    public void SetupCreate(RecipeId id, RecipeName name, NumberOfServings numberOfServings, IEnumerable<IIngredient> ingredients,
        IEnumerable<IPreparationStep> steps, IEnumerable<RecipeTagId> recipeTagIds, IRecipe returnValue)
    {
        Setup(m => m.Create(
                id,
                name,
                numberOfServings,
                It.Is<IEnumerable<IIngredient>>(list => list.IsEquivalentTo(ingredients)),
                It.Is<IEnumerable<IPreparationStep>>(list => list.IsEquivalentTo(steps)),
                It.Is<IEnumerable<RecipeTagId>>(list => list.IsEquivalentTo(recipeTagIds))))
            .Returns(returnValue);
    }
}