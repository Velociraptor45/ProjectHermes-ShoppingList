﻿using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes
{
    public class ModifyItemWithTypesContract
    {
        public ModifyItemWithTypesContract(string name, string comment, int quantityType, float? quantityInPacket,
            int? quantityTypeInPacket, Guid itemCategoryId, Guid? manufacturerId,
            IEnumerable<ModifyItemTypeContract> itemTypes)
        {
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            ItemTypes = itemTypes;
        }

        public string Name { get; set; }
        public string Comment { get; set; }
        public int QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public int? QuantityTypeInPacket { get; set; }
        public Guid ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public IEnumerable<ModifyItemTypeContract> ItemTypes { get; set; }
    }
}