﻿using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;

    public ItemCategoryEffects(IApiClient client, NavigationManager navigationManager)
    {
        _client = client;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleSearchItemCategoriesAction(SearchItemCategoriesAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.SearchInput))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(new List<ItemCategorySearchResult>()));
            return;
        }

        dispatcher.Dispatch(new SearchItemCategoriesStartedAction());

        var result = await _client.GetItemCategorySearchResultsAsync(action.SearchInput);

        var finishAction = new SearchItemCategoriesFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditItemCategoryAction(EditItemCategoryAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.ItemCategories}/{action.Id}");
        return Task.CompletedTask;
    }
}