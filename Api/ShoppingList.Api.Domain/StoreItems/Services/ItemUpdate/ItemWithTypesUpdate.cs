﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate
{
    public class ItemWithTypesUpdate
    {
        public ItemWithTypesUpdate(ItemId oldId, string name, string comment,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
            IEnumerable<ItemTypeUpdate> typeUpdates)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            OldId = oldId ?? throw new ArgumentNullException(nameof(oldId));
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId ?? throw new ArgumentNullException(nameof(itemCategoryId));
            ManufacturerId = manufacturerId;
            TypeUpdates = typeUpdates?.ToList() ?? throw new ArgumentNullException(nameof(typeUpdates));
        }

        public ItemId OldId { get; }
        public string Name { get; }
        public string Comment { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId? ManufacturerId { get; }
        public IReadOnlyCollection<ItemTypeUpdate> TypeUpdates { get; }
    }
}