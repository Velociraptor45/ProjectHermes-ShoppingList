﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults
{
    public class ItemFilterResultsQuery : IQuery<IEnumerable<ItemFilterResultReadModel>>
    {
        public ItemFilterResultsQuery(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
        {
            StoreIds = storeIds;
            ItemCategoriesIds = itemCategoriesIds;
            ManufacturerIds = manufacturerIds;
        }

        public IEnumerable<StoreId> StoreIds { get; }
        public IEnumerable<ItemCategoryId> ItemCategoriesIds { get; }
        public IEnumerable<ManufacturerId> ManufacturerIds { get; }
    }
}