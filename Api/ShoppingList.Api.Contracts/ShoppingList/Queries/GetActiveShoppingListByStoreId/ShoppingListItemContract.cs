﻿using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListItemContract
    {
        public ShoppingListItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityTypeContract quantityType, float quantityInPacket,
            QuantityTypeInPacketContract quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer, bool isInBasket, float quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new System.ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket ?? throw new System.ArgumentNullException(nameof(quantityTypeInPacket));
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityTypeContract QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacketContract QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}