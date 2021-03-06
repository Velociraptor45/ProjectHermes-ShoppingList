﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch
{
    public class ItemSearchReadModel
    {
        public ItemSearchReadModel(ItemId id, string name, int defaultQuantity, float price,
            ManufacturerReadModel manufacturer, ItemCategoryReadModel itemCategory,
            StoreSectionReadModel defaultSection)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            Manufacturer = manufacturer;
            ItemCategory = itemCategory;
            DefaultSection = defaultSection;
        }

        public ItemId Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public ManufacturerReadModel Manufacturer { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public StoreSectionReadModel DefaultSection { get; }
    }
}