﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemsState
    {
        private readonly List<Store> stores;
        private List<ItemCategory> itemCategories;
        private List<Manufacturer> manufacturers;
        private List<ItemFilterResult> items;
        private readonly List<QuantityType> quantityTypes;
        private readonly List<QuantityTypeInPacket> quantityTypesInPacket;

        public ItemsState(IEnumerable<Store> stores, IEnumerable<ItemCategory> itemCategories, IEnumerable<Manufacturer> manufacturers,
            IEnumerable<QuantityType> quantityTypes, IEnumerable<QuantityTypeInPacket> quantityTypesInPacket)
        {
            this.stores = stores.ToList();
            this.itemCategories = itemCategories.ToList();
            this.manufacturers = manufacturers.ToList();
            //this.items = items.ToList();
            this.quantityTypes = quantityTypes.ToList();
            this.quantityTypesInPacket = quantityTypesInPacket.ToList();
        }

        public IReadOnlyCollection<Store> Stores => stores.AsReadOnly();
        public IReadOnlyCollection<ItemCategory> ItemCategories => itemCategories.AsReadOnly();
        public IReadOnlyCollection<Manufacturer> Manufacturers => manufacturers.AsReadOnly();
        public IReadOnlyCollection<ItemFilterResult> Items => items.AsReadOnly();
        public IReadOnlyCollection<QuantityType> QuantityTypes => quantityTypes.AsReadOnly();
        public IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket => quantityTypesInPacket.AsReadOnly();

        public Func<Task> ManufacturerCreated { get; set; }

        public void UpdateManufacturers(IEnumerable<Manufacturer> manufacturers)
        {
            this.manufacturers = manufacturers.ToList();
        }
    }
}