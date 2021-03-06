﻿using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class MakeTemporaryItemPermanentRequest
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public MakeTemporaryItemPermanentRequest(int id, string name, string comment, int quantityType,
            float quantityInPacket, int quantityTypeInPacket, int itemCategoryId, int? manufacturerId,
            IEnumerable<StoreItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            this.availabilities = availabilities;
        }

        public int Id { get; }
        public string Name { get; }
        public string Comment { get; }
        public int QuantityType { get; }
        public float QuantityInPacket { get; }
        public int QuantityTypeInPacket { get; }
        public int ItemCategoryId { get; }
        public int? ManufacturerId { get; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();
    }
}