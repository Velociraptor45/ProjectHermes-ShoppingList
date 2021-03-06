﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch
{
    public class ItemSearchQuery : IQuery<IEnumerable<ItemSearchReadModel>>
    {
        public ItemSearchQuery(string searchInput, StoreId storeId)
        {
            SearchInput = searchInput;
            StoreId = storeId;
        }

        public string SearchInput { get; }
        public StoreId StoreId { get; }
    }
}