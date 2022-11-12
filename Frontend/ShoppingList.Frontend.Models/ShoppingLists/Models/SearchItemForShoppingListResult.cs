﻿using System;
using System.Globalization;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class SearchItemForShoppingListResult
    {
        private static readonly CultureInfo _culture = new("de-de");

        public SearchItemForShoppingListResult(Guid itemId, Guid? itemTypeId, string name, float price,
            int defaultQuantity, string priceLabel, string itemCategoryName, string manufacturerName,
            Guid defaultSectionId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Name = name;
            Price = price;
            DefaultQuantity = defaultQuantity;
            PriceLabel = priceLabel;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSectionId = defaultSectionId;
        }

        public Guid ItemId { get; set; }
        public Guid? ItemTypeId { get; }
        public string Name { get; }
        public float Price { get; }
        public int DefaultQuantity { get; }
        public string PriceLabel { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
        public Guid DefaultSectionId { get; }

        public string DisplayValue
        {
            get => string.IsNullOrWhiteSpace(ManufacturerName)
                ? $"{Name} | {Price.ToString(_culture)}{PriceLabel}"
                : $"{Name} | {ManufacturerName} | {Price.ToString(_culture)}{PriceLabel}";
            set { _ = value; }
        }

        public string SelectIdentifier
        {
            get => $"{ItemId}{ItemTypeId?.ToString() ?? string.Empty}";
            set { _ = value; }
        }
    }
}