﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries
{
    public class ItemCategorySearchResultContract
    {
        public ItemCategorySearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}