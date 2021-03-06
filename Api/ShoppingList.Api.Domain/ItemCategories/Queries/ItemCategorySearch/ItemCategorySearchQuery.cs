﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.ItemCategorySearch
{
    public class ItemCategorySearchQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
    {
        public ItemCategorySearchQuery(string searchInput)
        {
            SearchInput = searchInput;
        }

        public string SearchInput { get; }
    }
}