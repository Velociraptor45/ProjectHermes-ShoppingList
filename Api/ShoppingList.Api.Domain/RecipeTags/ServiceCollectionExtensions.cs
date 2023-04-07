﻿using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Query;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags;

internal static class ServiceCollectionExtensions
{
    public static void AddRecipeTags(this IServiceCollection services)
    {
        services.AddTransient<IRecipeTagFactory, RecipeTagFactory>();

        services.AddTransient<Func<CancellationToken, IRecipeTagQueryService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            return token => new RecipeTagQueryService(repo, token);
        });
    }
}