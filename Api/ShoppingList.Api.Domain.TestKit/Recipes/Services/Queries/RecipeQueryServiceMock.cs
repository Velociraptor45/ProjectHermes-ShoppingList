﻿using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Queries;

public class RecipeQueryServiceMock : Mock<IRecipeQueryService>
{
    public RecipeQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSearchByNameAsync(string searchInput, IEnumerable<RecipeSearchResult> returnValue)
    {
        Setup(m => m.SearchByNameAsync(searchInput)).ReturnsAsync(returnValue);
    }

    public void SetupGetAsync(RecipeId recipeId, IRecipe returnValue)
    {
        Setup(m => m.GetAsync(recipeId)).ReturnsAsync(returnValue);
    }
}