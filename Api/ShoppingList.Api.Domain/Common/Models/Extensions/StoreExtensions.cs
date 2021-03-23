﻿using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static ShoppingListStoreReadModel ToShoppingListStoreReadModel(this IStore model)
        {
            return new ShoppingListStoreReadModel(model.Id, model.Name);
        }

        public static StoreReadModel ToActiveStoreReadModel(this IStore model,
            IEnumerable<StoreItemReadModel> items)
        {
            return new StoreReadModel(model.Id, model.Name, items, model.Sections.Select(s => s.ToReadModel()));
        }
    }
}