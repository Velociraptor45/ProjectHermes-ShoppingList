﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;

public class SearchItemsForShoppingListQuery : IQuery<IEnumerable<SearchItemForShoppingResultReadModel>>
{
    public SearchItemsForShoppingListQuery(string searchInput, StoreId storeId)
    {
        SearchInput = searchInput;
        StoreId = storeId;
    }

    public string SearchInput { get; }
    public StoreId StoreId { get; }
}