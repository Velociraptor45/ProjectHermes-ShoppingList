﻿using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;

public interface IRecipeTagRepository
{
    Task<IRecipeTag> StoreAsync(IRecipeTag recipeTag);
}